using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ItemCountInventory : MonoBehaviour, INotifyPropertyChanged
{
    public TextMeshProUGUI CountText;
    private string _coutText;

    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public string Cout 
    { 
        get => _coutText; 
        set
        {
            _coutText = value;
            OnProperyChanged("Cout");
        } 
    }

    private void OnProperyChanged(string propertyName)
    {
        if(PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
