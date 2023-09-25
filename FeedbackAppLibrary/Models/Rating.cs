using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackAppLibrary.Models;
public enum Rating {
  Poor=1,
  Fair=2,
  Good=3,
  [EnumMember(Value = "Very Good")]
  VeryGood=4,
  Excellent=5
}
