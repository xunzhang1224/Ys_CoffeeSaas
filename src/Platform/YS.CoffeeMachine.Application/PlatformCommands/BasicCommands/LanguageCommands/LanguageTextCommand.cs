using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands
{
    public record CreateLanguageTextCommand(string code, Dictionary<string, string> languageValue) : ICommand;

    public record DeleteLanguageTextCommand(string code) : ICommand;

    //public record UpdateLanguageTextCommand(string code, DictionaryEntity<string, string> languageValue) : ICommand;

    public record ImportSysLanguageTextCommand(string LangCode, Dictionary<string, string> dic) : ICommand;

    //public record ExportSysLanguageTextCommand( string langCode, string fileName) : ICommand<bool>;
}
