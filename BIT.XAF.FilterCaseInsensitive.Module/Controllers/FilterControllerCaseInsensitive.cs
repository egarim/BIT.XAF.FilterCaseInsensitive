using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BIT.XAF.FilterCaseInsensitive.Module.Controllers
{
    public class FilterControllerCaseInsensitive : ViewController<ListView>
    {
        public FilterControllerCaseInsensitive()
        {

        }
        FilterController standardFilterController;
        protected override void OnActivated()
        {
            standardFilterController = Frame.GetController<FilterController>();
            if (standardFilterController == null)
                return;

            //we should wire the execution of the filter
            standardFilterController.FullTextFilterAction.Execute += FullTextFilterAction_Execute;



        }



        private void FullTextFilterAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            //we locate filter with the key FilterController.FullTextSearchCriteriaName then we convert it to case insensitive
            if (!string.IsNullOrEmpty(e.ParameterCurrentValue as string) && View.CollectionSource.Criteria.ContainsKey(FilterController.FullTextSearchCriteriaName))
                View.CollectionSource.Criteria[FilterController.FullTextSearchCriteriaName] = GetCaseInsensitiveCriteria(e.ParameterCurrentValue, View.CollectionSource.Criteria[FilterController.FullTextSearchCriteriaName]);
        }

        private CriteriaOperator GetCaseInsensitiveCriteria(object searchValue, CriteriaOperator initialCriteria)
        {

            //we get a list of all the properties that can be involved in the filter
            var SearchProperties = standardFilterController.GetFullTextSearchProperties();
            //we declare a model class and a property name,the values on this variables will change if we property involve is a navigation property (another persistent object)
            IModelClass ModelClass = null;
            string PropertyName = string.Empty;

            //we declare a list of operators to contains new operators we are going to create
            List<CriteriaOperator> Operator = new List<CriteriaOperator>();
            //we iterate all the properties
            foreach (var CurrentProperty in SearchProperties)
            {
                //here we split the name with a dot, if length is greater than 1 it means its a navigation property, beware that this may fail with a deep tree of properties like category.subcategory.name
                var Split = CurrentProperty.Split('.');
                if (Split.Length > 1)
                {

                    Debug.WriteLine(string.Format("{0}", "its a complex property"));
                    var CurrentClass = this.View.Model.ModelClass;
                    for (int i = 0; i < Split.Length; i++)
                    {

                        //if its a navigation property we locate the type in the BOModel
                        //IModelMember member = CurrentClass.OwnMembers.Where(m => m.Name == Split[i]).FirstOrDefault();
                        //var member = CurrentClass.AllMembers.Where(m => m.Name == Split[i]).FirstOrDefault();
                        IModelMember member = CurrentClass.AllMembers.Where(m => m.Name == Split[i]).FirstOrDefault();

                        //then we set the model class and property name to the values of the navigation property like category.name where category is the model class and name is the property
                        CurrentClass = this.Application.Model.BOModel.GetClass(member.Type);
                        if (CurrentClass == null)
                            continue;

                        ModelClass = CurrentClass;
                        PropertyName = Split[i + 1];


                    }
                    Debug.WriteLine(string.Format("{0}:{1}", "ModelClass", ModelClass.Name));
                    Debug.WriteLine(string.Format("{0}:{1}", "PropertyName", PropertyName));

                   


                }
                else
                {
                    //else the model class will be the current class where the filter is executing, and the property will be the current property we are evaluating
                    ModelClass = this.View.Model.ModelClass;
                    PropertyName = CurrentProperty;
                }

                //we look for the property on the class model own member
                var Property = ModelClass.OwnMembers.Where(m => m.Name == PropertyName).FirstOrDefault();
                if (Property != null)
                {
                    //if the property is a string it means that we can set it to upper case
                    if (Property.Type == typeof(string))
                    {
                        searchValue = searchValue.ToString().ToUpper();
                        //we create an operator where we set the value of the property to upper before we compare it, also we change the comparison value to upper
                        CriteriaOperator Operand = CriteriaOperator.Parse("Contains(Upper(" + CurrentProperty + "), ?)", searchValue);
                        //we added to the list of operators that will concatenate with OR
                        Operator.Add(Operand);
                    }
                    else
                    {
                        //if the property is not a string we need to try to cast the value to the correct type so we do a catch try, if we manage to cast the value it will be added to the operators list
                        try
                        {

                            var ConvertedType = Convert.ChangeType(searchValue, Property.Type);
                            CriteriaOperator operand = new BinaryOperator(CurrentProperty, ConvertedType, BinaryOperatorType.Equal);
                            Operator.Add(operand);
                        }
                        catch (Exception)
                        {

                            //silent exception, this will happen if the casting was not successfully so we won't add the operand on this case
                        }
                    }




                }
            }

            //we concatenate everything with an OR 
            var alloperators = CriteriaOperator.Or(Operator.ToArray());
            Debug.WriteLine(string.Format("{0}:{1}", "all operators", alloperators));
            return alloperators;
        }
    }
}
