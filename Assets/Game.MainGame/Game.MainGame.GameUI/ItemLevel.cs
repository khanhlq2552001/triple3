using System.Collections.Generic;
using BlitzyUI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class ItemLevel : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objStarsDone;
        [SerializeField] private Text _txtLevel;
        [SerializeField] private Text _txtLevel1;
        [SerializeField] private Text _txtLevel2;

        [SerializeField] private GameObject _objStar;
        [SerializeField] private Image _imgBG;
        [SerializeField] private Sprite _sprChoose;
        [SerializeField] private Sprite _sprLock;
        [SerializeField] private Sprite _sprDontLock;
        [SerializeField] private Button _btn;

        public int type;
        public int level;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void Start()
        {
            _btn.onClick.AddListener(() => Btn());
        }

        public void SetStar(int countStar)
        {
            for(int i=0; i< _objStarsDone.Count; i++)
            {
                if(i < countStar)
                {
                    _objStarsDone[i].SetActive(true);
                }
                else
                {
                    _objStarsDone[i].SetActive(false);
                }
            }
        }

        public void ResetItem()
        {
            for(int i=0; i< _objStarsDone.Count; i++)
            {
                _objStarsDone[i].SetActive(false);
            }
        }

        public void SetType(int type, int level)
        {
            this.type = type;
            _txtLevel.text = level.ToString();
            _txtLevel1.text = level.ToString();
            _txtLevel2.text = level.ToString();

            bool isOk = false;
            if (_txtLevel2.gameObject.activeSelf)
            {
                isOk = true;
            }
            _txtLevel.gameObject.SetActive(false);
            _txtLevel1.gameObject.SetActive(false);
            _txtLevel2.gameObject.SetActive(false);

            this.level = level;


            if (type == 0)
            {
                _imgBG.sprite = _sprLock;
                _objStar.SetActive(false);
              //  _txtLevel.gameObject.SetActive(false);
                _txtLevel1.SetActive(true);
            }
            else if (type == 1)
            {
                _imgBG.sprite = _sprDontLock;
                _objStar.SetActive(true);
                _txtLevel.SetActive(true);
              //  _txtLevel.gameObject.SetActive(true);
                
            }
            else if (type == 2)
            {
                _imgBG.sprite = _sprChoose;
                if (!isOk)
                    _txtLevel.SetActive(true);
                else _txtLevel2.SetActive(true);
            }
            else if(type == 3)
            {
                _imgBG.sprite = _sprDontLock;
            //    _txtLevel.gameObject.SetActive(true);
                _objStar.SetActive(false);
                _txtLevel2.SetActive(true);
            }
        }

        public void Btn()
        {
            if(type == 1 || type == 3)
            {
                SetType(2, level);
                UIHome ui = UIManager.Instance.GetScreen<UIHome>(GameManager.ScreenID_Home);
                ui.SetItemChoose(this, level);
            }
        }
    }
}
