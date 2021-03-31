using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class RfSystem
{
    private Element _Switch; 
    private Element _LNA;
    private Element _Mixer;
    private Element _Filter;
    // сюда только передаются элементы, это конструктор класса
    public RfSystem(Element Switches, Element LNA, Element Mixer, Element Filter)     
    {
        _Switch = Switches;
        _LNA = LNA;
        _Mixer = Mixer;
        _Filter = Filter;
    }
    // а расчет нужно делать вот так, через методы:
    public string SystemIndexes()
    {
        return _Switch.index + " " + _LNA.index + " " + _Mixer.index + " " + _Filter.index;
    }

    public Element SystemSwitch()
    {
        return _Switch;
    }

    public Element SystemLNA()
    {
        return _LNA;
    }
    public Element SystemMixer()
    {
        return _Mixer;
    }
    public Element SystemFilter()
    {
        return _Filter;
    }





    public string SystemDescription()
    {
        return  _Switch.name + " " + _LNA.name + " " + _Mixer.name + " " + _Filter.name;
    }
    public double TotalGain()
    {
        return  _Switch.gain + _LNA.gain + _Mixer.gain + _Filter.gain;
    }
    public double TotalCost()
    {
        return _Switch.cost + _LNA.cost + _Mixer.cost + _Filter.cost;
    }
    public double TotalNoise() //c формулой
    {
        double _switchnoise = _Switch.noise;
        double _switchgain = _Switch.gain;

        double _LNAnoise = _LNA.noise;
        double _LNAgain = _LNA.gain;

        double _mixernoise = _Mixer.noise;
        double _mixergain = _Mixer.gain;

        double _filternoise = _Filter.noise;

        _switchgain = Math.Pow(10, (_switchgain / 10));
        _LNAgain = Math.Pow(10, (_LNAgain / 10));
        _mixergain = Math.Pow(10, (_mixergain / 10));


        _switchnoise = Math.Pow(10, (_switchnoise / 10));
        _LNAnoise = Math.Pow(10, (_LNAnoise / 10));
        _mixernoise = Math.Pow(10, (_mixernoise / 10));
        _filternoise = Math.Pow(10, (_filternoise / 10));
        
        return Math.Round(10 * Math.Log10(_switchnoise + ((_LNAnoise - 1) / _switchgain) + ((_mixernoise - 1) / (_switchgain * _LNAgain)) + ((_filternoise - 1) / (_switchgain * _LNAgain * _mixergain))), 3);
    }        
}
  
