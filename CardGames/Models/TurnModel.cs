using System.ComponentModel.DataAnnotations;
using CardGames.Core.Durak;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CardGames.Models
{
    public class TurnModel
    {
        [Required]
        public Card Card { get; set; }

        public Card Target { get; set; }
    }
}