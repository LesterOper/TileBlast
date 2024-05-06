using CodeBase.CoreBase.Configs;

namespace CodeBase.CoreBase.CellBase
{
    public interface IFieldCell
    {
        ICellBase Cell { get; set; }
        FieldPosition FieldPositionProperty { get; }
        
        bool IsBooster { get; }
        bool IsEmpty { get; }
        bool IsNonActive { get; }
        bool IsDropDown { get; }
        bool IsBlocked { get; }
        
        bool IsDefaultCell { get; }
        
    }
    
    public class FieldCell : IFieldCell
    {
        private ICellBase _cellBase;
        private FieldPosition _fieldPosition;

        private bool _isBooster;
        private bool _isEmpty;
        private bool _isNonActive;
        private bool _isDropDown;
        private bool _isBlocked;
        private bool _isDefaultCell;

        public FieldCell(FieldPosition fieldPosition)
        {
            _fieldPosition = fieldPosition;
        }

        public ICellBase Cell
        {
            get => _cellBase;
            set
            {
                _cellBase = value;

                if (_cellBase is IBooster)
                {
                    _isBooster = true;
                    _isDefaultCell = _isBlocked = _isEmpty = _isDropDown = _isNonActive = false;
                    return;
                }

                if (_cellBase is ICell)
                {
                    _isDefaultCell = true;
                    _isBooster = _isBlocked = _isEmpty = _isDropDown = _isNonActive = false;
                    return;
                }

                if (_cellBase is INotActiveCell)
                {
                    _isNonActive = true;
                    _isBooster = _isBlocked = _isEmpty = _isDropDown = _isDefaultCell = false;
                    return;
                }

                if (_cellBase is IDropDownItem)
                {
                    _isDropDown = true;
                    _isBooster = _isBlocked = _isEmpty = _isNonActive = _isDefaultCell = false;
                    return;
                }

                if (_cellBase is IBlockerCell)
                {
                    _isBlocked = true;
                    _isBooster = _isDropDown = _isEmpty = _isNonActive = _isDefaultCell = false;
                    return;
                }

                if (_cellBase is IEmptyCell)
                {
                    _isEmpty = true;
                    _isBooster = _isDropDown = _isBlocked = _isNonActive = _isDefaultCell = false;
                    return;
                }
            }
        }

        public FieldPosition FieldPositionProperty => _fieldPosition;

        public bool IsBooster => _isBooster;
        public bool IsEmpty => _isEmpty;
        public bool IsNonActive => _isNonActive;
        public bool IsDropDown => _isDropDown;
        public bool IsBlocked => _isBlocked;
        public bool IsDefaultCell => _isDefaultCell;
    }
}