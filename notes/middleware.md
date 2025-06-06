# 6.06.25
- created TaskItemValidator to validate title length and isComplete value
- registered TaskItemValidator
- added validator parameter to POST and PUT invocations, invoked ValidateAndThrow(dto) and removed manual checking of title
- added ErrorHandlingMiddleware to handle errors