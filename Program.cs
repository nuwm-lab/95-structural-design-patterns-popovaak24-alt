using System;

// Контракт колісного трактора
interface IWheeledTractor
{
	int WheelsCount { get; }
	int HorsePower { get; }
	void Drive();
	void AttachPlow();
}
// Контракт гусеничного транспортного засобу (клієнт очікує такий інтерфейс)
interface ITrackedVehicle
{
	string TrackType { get; }
	double TreadWidth { get; }
	void DriveOnSoftTerrain();
	void OperatePlow();
}
// Конкретна реалізація колісного трактора
class WheeledTractor : IWheeledTractor
{
	public int WheelsCount { get; private set; }
	public int HorsePower { get; private set; }

	public WheeledTractor(int wheels, int hp)
	{
		WheelsCount = wheels;
		HorsePower = hp;
	}

	public void Drive()
	{
		Console.WriteLine($"[WheeledTractor] Рухається з {WheelsCount} колесами і потужністю {HorsePower} к.с.");
	}

	public void AttachPlow()
	{
		Console.WriteLine("[WheeledTractor] Причеплено плуг (для роботи на твердій поверхні).");
	}
}

// Адаптер: перетворює IWheeledTractor на ITrackedVehicle
class TractorToTrackedAdapter : ITrackedVehicle
{
	private readonly IWheeledTractor _wheeledTractor;
	public string TrackType { get; private set; }
	public double TreadWidth { get; private set; }

	public TractorToTrackedAdapter(IWheeledTractor wheeledTractor, string trackType = "Резино-металева", double treadWidth = 0.5)
	{
		_wheeledTractor = wheeledTractor ?? throw new ArgumentNullException(nameof(wheeledTractor));
		TrackType = trackType;
		TreadWidth = treadWidth;
	}

	// Адаптовано: використаємо внутрішній метод Drive як базу, але змінимо повідомлення
	public void DriveOnSoftTerrain()
	{
		// Можна додати логіку, яка зменшує швидкість або обчислює прохідність на основі HorsePower
		Console.WriteLine($"[Adapter] Перетворюємо колеса на гусениці ({TrackType}, ширина {TreadWidth} м). Подвійна перевірка потужності...");
		_wheeledTractor.Drive();
		Console.WriteLine("[TrackedVehicle] Тепер техніка рухається по м'якому ґрунту як гусенична.");
	}

	// Адаптований виклик для плуга
	public void OperatePlow()
	{
		Console.WriteLine("[Adapter] Налаштовуємо кріплення плуга для гусеничного режиму...");
		_wheeledTractor.AttachPlow();
		Console.WriteLine("[TrackedVehicle] Плуг працює стабільніше завдяки гусеницям.");
	}
}

class Program
{
	static void Main()
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;

		// Демонстрація: маємо колісний трактор
		IWheeledTractor wheeled = new WheeledTractor(wheels: 4, hp: 120);
		Console.WriteLine("=== Стан: колісний трактор ===");
		wheeled.Drive();
		wheeled.AttachPlow();

		Console.WriteLine();

		// Але клієнт очікує гусеничний інтерфейс — використовуємо адаптер
		ITrackedVehicle tracked = new TractorToTrackedAdapter(wheeled, trackType: "Сталева гусениця", treadWidth: 0.6);
		Console.WriteLine("=== Після адаптації: трактор як гусеничний ===");
		tracked.DriveOnSoftTerrain();
		tracked.OperatePlow();

		Console.WriteLine();
		Console.WriteLine("Демонстрація завершена. Натисніть Enter для виходу...");
		Console.ReadLine();
	}
}

