using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace db_mapping
{
    /// <summary>
    /// Summary description for Ingredient
    /// </summary>
    public class Ingredient
    {
        public static int UninitializedInt = -100000;

        public Ingredient()
        {
            id_ = UninitializedInt;
            denumire_ = null;
        }

        public void Initialize(int id, String denumire)
        {
            id_ = id;
            denumire_ = denumire;
        }

        public int Id
        {
            get { return id_; }
        }

        public String Denumire
        {
            get { return denumire_; }
        }

        public bool IsNew
        {
            get { return id_ == UninitializedInt; }
        }

        private int id_;
        private String denumire_;
    }
} // namespace
