using MakesEasy.Models;

namespace MakesEasy.Interfaces;

public interface IMessageInterface
{
    Task<int> PostMessage(MessageModel msg);
}