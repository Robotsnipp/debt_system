from django.db import models
from django.core.exceptions import ValidationError
from datetime import datetime


class Person(models.Model):
    """Абстрактный базовый класс для человека (должника или кредитора)."""
    first_name = models.CharField(max_length=100, verbose_name="Имя")
    last_name = models.CharField(max_length=100, verbose_name="Фамилия")
    email = models.EmailField(blank=True, default='', verbose_name="Email")

    class Meta:
        abstract = True

    @property
    def full_name(self):
        return f"{self.first_name} {self.last_name}"

    def clean(self):
        if not self.first_name or not self.first_name.strip():
            raise ValidationError("Имя не может быть пустым.")
        if not self.last_name or not self.last_name.strip():
            raise ValidationError("Фамилия не может быть пустой.")

    def __str__(self):
        return self.full_name


class Debtor(Person):
    """Должник."""
    class Meta:
        verbose_name = "Должник"
        verbose_name_plural = "Должники"


class Creditor(Person):
    """Кредитор."""
    class Meta:
        verbose_name = "Кредитор"
        verbose_name_plural = "Кредиторы"


class Debt(models.Model):
    """Базовый класс долга."""
    debtor = models.ForeignKey(Debtor, on_delete=models.CASCADE, related_name='debts', verbose_name="Должник")
    creditor = models.ForeignKey(Creditor, on_delete=models.CASCADE, related_name='debts', verbose_name="Кредитор")
    
    amount = models.DecimalField(max_digits=12, decimal_places=2, verbose_name="Сумма долга")
    remaining_amount = models.DecimalField(max_digits=12, decimal_places=2, verbose_name="Остаток долга")
    
    issue_date = models.DateTimeField(auto_now_add=True, verbose_name="Дата выдачи")
    due_date = models.DateTimeField(null=True, blank=True, verbose_name="Срок погашения")
    
    debt_type = models.CharField(max_length=50, default='personal_loan', verbose_name="Тип долга")

    class Meta:
        verbose_name = "Долг"
        verbose_name_plural = "Долги"

    def clean(self):
        if self.amount < 0:
            raise ValidationError("Сумма долга должна быть положительной.")

    @property
    def is_paid(self):
        return self.remaining_amount <= 0

    def calculate_penalty(self):
        """Расчет штрафа для личных займов."""
        if not self.due_date or datetime.now() <= self.due_date:
            return 0
        
        days_late = (datetime.now() - self.due_date).days
        if days_late <= 0:
            return 0
        
        months_late = (days_late + 29) // 30  # Округление вверх
        return self.amount * 0.05 * months_late

    def save(self, *args, **kwargs):
        if not self.pk:
            self.remaining_amount = self.amount
        self.clean()
        super().save(*args, **kwargs)

    def __str__(self):
        return f"Долг {self.debtor.full_name} -> {self.creditor.full_name}: {self.amount}"


class Payment(models.Model):
    """Выплата по долгу."""
    debt = models.ForeignKey(Debt, on_delete=models.CASCADE, related_name='payments', verbose_name="Долг")
    amount = models.DecimalField(max_digits=12, decimal_places=2, verbose_name="Сумма выплаты")
    payment_date = models.DateTimeField(auto_now_add=True, verbose_name="Дата выплаты")

    class Meta:
        verbose_name = "Выплата"
        verbose_name_plural = "Выплаты"

    def clean(self):
        if self.amount <= 0:
            raise ValidationError("Сумма выплаты должна быть положительной.")

    def save(self, *args, **kwargs):
        self.clean()
        super().save(*args, **kwargs)
        
        # Обновляем остаток долга
        debt = self.debt
        debt.remaining_amount -= self.amount
        debt.save()

    def __str__(self):
        return f"Выплата {self.amount} по долгу {self.debt.id}
