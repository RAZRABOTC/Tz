# Паттерны
Dependency Injection - GameInstaller, [Inject] во всех классах	FirstPersonMovement, QuestNPC, DialogueUIManager ... получают зависимости через конструктор
Singleton -	PlayerItemsController	Instance для доступа к текущему предмету игрока
Abstract Factory -	Speed.AddModifier()	Создание модификаторов скорости (SpeedModifier)
Strategy -	Speed + SpeedModifier	Разные модификаторы скорости (приседание, бег) применяются через FinalSpeed
Observer - InputHandler	События OnJump, OnCrouch, OnInteraction
Command	- DialogueUIManager	Кнопки диалога хранят onAnswerSelected?.Invoke() как команду
Adapter	- InputHandler Адаптирует UnityEngine.InputSystem к собственным событиям
Composite	- Speed	Несколько SpeedModifier собираются в один FinalSpeed
# Архитектура
1. Разделение UI и логики
- DialogueUIManager ничего не знает о NPC. Он просто показывает сообщения и вызывает переданные делегаты. Это позволяет переиспользовать UI для любых диалогов.
2. Абстрактный BaseNPC
- Базовый класс содержит общую логику (подписки, Update, Interact), а конкретные реализации (NonlinearNPC, QuestNPC) переопределяют только специфичное поведение.
3. Система модификаторов скорости (Speed + SpeedModifier)
- Позволяет динамически добавлять эффекты, влияющие на скорость (приседание, бег, замедление в луже) без изменения основного класса FirstPersonMovement.
4. InteractMachine с поддержкой удержания
- Обрабатывает два типа взаимодействия: обычное (IInteractable) и с удержанием (ValveInteractable). Разные подсказки и разная логика.
5. ValveInteractable с плавным возвратом через DoTween
- Вращение пропорционально времени удержания, дверь открывается синхронно. При отпускании — плавный возврат с анимацией.
6. Централизованный InputHandler через New Input System
- Все действия (прыжок, приседание, взаимодействие, ...) проходят через один обработчик. Легко менять привязки клавиш.
7. PlayerController.Toggle(bool)
- Позволяет отключать всё управление игроком одной строкой — для меню, диалогов, пауз.
8. InteractableWithConditions
- Позволяет создавать интерактивные объекты, требующие определённый предмет в руках (_neededItem), с опцией одноразового использования (_onlyOnce).
