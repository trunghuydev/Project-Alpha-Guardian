using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameSkill : MonoBehaviour
{
    public GameObject panelskill;
    public GameObject panelinfo;
    public Button xayahskillButton;
    public Button rakanskillButton; 
    public GameObject panelrakan;
    public GameObject panelxayah;

    public Button NoiTairakan;
    public Button Skill1rakan;
    public Button Skill2rakan;
    public Button Skill3rakan;

    public Button NoiTaixayah;
    public Button Skill1xayah;
    public Button Skill2xayah;
    public Button Skill3xayah;

    public TextMeshProUGUI skillDtRakan; // Text UI cho mô tả kỹ năng của Rakan
    public TextMeshProUGUI skillDescriptionTextXayah; // Text UI cho mô tả kỹ năng của Xayah
    public TextMeshProUGUI skillNameTextRakan; // Text UI cho tên kỹ năng của Rakan
    public TextMeshProUGUI skillNameTextXayah; // Text UI cho tên kỹ năng của Xayah

    public HeroInfo heroInfo;

    private bool isRakanSelected = false; 
    private bool isXayahSelected = false; 


    void Start()
    {
        SetUpSkillButtons();
    }
    public void ShowThongTin()
    {
        panelinfo.SetActive(true);
        panelskill.SetActive(false);
        if (heroInfo != null)
        {
           
            heroInfo.showRakanFilesButton.gameObject.SetActive(true);
            heroInfo.showXayahFilesButton.gameObject.SetActive(true);
        }
    }

    public void ShowKyNang()
    {
        panelinfo.SetActive(false);
        panelskill.SetActive(true);
        if (heroInfo != null)
        {
          
            heroInfo.showRakanFilesButton.gameObject.SetActive(false);
            heroInfo.showXayahFilesButton.gameObject.SetActive(false);
        }
    }

    public void ShowXayahSkill()
    {
        
        panelxayah.SetActive(true);
        panelrakan.SetActive(false);

        
        isXayahSelected = true;
        isRakanSelected = false;
    }

    public void ShowRakanSkill()
    {
        
        panelrakan.SetActive(true);
        panelxayah.SetActive(false);

        // Cập nhật trạng thái
        isRakanSelected = true;
        isXayahSelected = false;
    }

    // Hàm để cập nhật tên và mô tả kỹ năng cho Rakan khi nhấn nút
    public void ShowSkillDescriptionRakan(int skillNumber)
    {
        if (isRakanSelected) 
        {
            switch (skillNumber)
            {
                case 1:
                    skillNameTextRakan.text = "Tấn công thường - Áo choàng Lông vũ";
                    skillDtRakan.text = "Tấn công kẻ địch mỗi 1 giây, gây </color=yellow>90% sát thương Phong </color>cho toàn bộ kẻ địch trúng chiêu";
                    break;
                case 2:
                    skillNameTextRakan.text = "Chiến kĩ - Công kích hoành tráng";
                    skillDtRakan.text = "Lao về hướng chỉ định, đẩy lùi toàn bộ kẻ địch trúng phải và gây </color=yellow>160% sát thương Phong</color>. \nMỗi kẻ địch trúng chiêu cho Rakan lá chắn tương đương </color=yellow>6 % máu tối đa +75</color>. \nHồi năng lượng: 4 + 2 với mỗi kẻ địch trúng chiêu";
                    break;
                case 3:
                    skillNameTextRakan.text = "Tuyệt kĩ - Vũ điệu tuyệt thế";
                    skillDtRakan.text = "Nhận 1 năng lượng mỗi 2 giây, mỗi khi bị tấn công hoặc tấn công thường. Khi năng lượng đạt 100 có thể kích hoạt:\nTiến vào trạng thái Hào hứng trong 12 giây, trong trạng thái Hào hứng:\nRakan nhận </color=yellow>50 % Tốc độ di chuyển</color>, tạo ra một vùng đất bao quanh bản thân.Vùng đất này cho Rakan <color=yellow> 30 % miễn thương</color>, gây <color=yellow> 150 % sát thương Phong </color>cho kẻ địch xung quanh mỗi giây, nhận lá chắn tương đương <color=yellow> 4% máu tối đa mỗi giây +50</color> với mỗi kẻ địch trúng chiêu.\nTrong trạng thái Hào hứng, không thể nhận năng lượng,tấn công thường hoặc Chiến kỹ cho đến khi trạng thái này kết thúc.";
                    break;
                case 4:
                    skillNameTextRakan.text = "Nội tại - Lá chắn tự nhiên:";
                    
                    skillDtRakan.text = "Khi Rakan nhận lá chắn, Xayah cũng nhận lá chắn bằng <color=yellow> 50% lượng lá chắn</color> đó.Khi xuất trận hoặc mỗi 40 giây, Rakan cho bản thân lá chắn tương đương <color=yellow> 18 % máu tối đa +150</color>. Các lá chắn của Rakan có thể cộng dồn và tồn tại cho đến khi bị phá vỡ.";
                    break;
            }
        }
    }

    // Hàm để cập nhật tên và mô tả kỹ năng cho Xayah khi nhấn nút
    public void ShowSkillDescriptionXayah(int skillNumber)
    {
        if (isXayahSelected) 
        {
            switch (skillNumber)
            {
                case 1:
                    skillNameTextXayah.text = "Tấn công thường - Phi dao";
                    skillDescriptionTextXayah.text = "Phóng lông vũ mỗi 1.2 giây, gây <color=yellow> 90% sát thương Hỏa</color> lên kẻ địch đầu tiên và <color=yellow> 45% sát thương Hỏa</color> toàn bộ kẻ địch trúng chiêu sau đó.";
                    break;
                case 2:
                    skillNameTextXayah.text = "Chiến kĩ - Nước dao đôi";
                    skillDescriptionTextXayah.text = "Ngay lập tức tung 2 lông vũ về phía trước, gây <color=yellow> 60 sát thương Hỏa</color> mỗi lông vũ.\nSau đó gọi các lông vũ về.";
                    break;
                case 3:



                    skillNameTextXayah.text = "Tuyệt kĩ - Bão lông vũ";
                    skillDescriptionTextXayah.text = "Nhận 1 năng lượng mỗi 2 giây và nhận 1 năng lượng sau mỗi đòn đánh. Khi đủ 90 năng lượng có thể thi triển Tuyệt kĩ:\nXayah nhảy lên, miễn sát thương và khống chế, đồng thời phóng ra 7 lông vũ về phía trước, mỗi lông vũ gây <color=yellow> 60% sát thương Hỏa</color> lên toàn bộ kẻ địch trúng chiêu.\nSau đó, cô gọi các lông vũ về.";
                    break;
                case 4:
                   

                    skillNameTextXayah.text = "Nội tại - Dấu tích thợ săn:";
                    skillDescriptionTextXayah.text = "Xayah có thể triệu hồi lông vũ thông qua đòn đánh hoặc kĩ năng của cô.\nKhi gọi các Lông vũ trên sân về, mỗi lông vũ gây <color=yellow> 60 sát thương Hỏa </color>lên kẻ địch trúng chiêu.\nLông vũ tồn tại 10 giây kể từ khi cắm xuống đất";
                    break;
            }
        }
    }

    // Cập nhật mô tả và tên kỹ năng cho các nút của Rakan và Xayah
    public void SetUpSkillButtons()
    {
        // Các nút Rakan
        Skill1rakan.onClick.AddListener(() => ShowSkillDescriptionRakan(1));
        Skill2rakan.onClick.AddListener(() => ShowSkillDescriptionRakan(2));
        Skill3rakan.onClick.AddListener(() => ShowSkillDescriptionRakan(3));
        NoiTairakan.onClick.AddListener(() => ShowSkillDescriptionRakan(4));

        // Các nút Xayah
        Skill1xayah.onClick.AddListener(() => ShowSkillDescriptionXayah(1));
        Skill2xayah.onClick.AddListener(() => ShowSkillDescriptionXayah(2));
        Skill3xayah.onClick.AddListener(() => ShowSkillDescriptionXayah(3));
        NoiTaixayah.onClick.AddListener(() => ShowSkillDescriptionXayah(4));
    }
}
