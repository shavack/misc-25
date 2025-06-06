# 6.06.25
- created TaskItemValidator to validate title length and isComplete value
- registered TaskItemValidator as IValidator<TaskItemDto>
- added validator parameter to POST and PUT invocations, invoked ValidateAndThrow(dto) and removed manual checking of title
- added exception handler