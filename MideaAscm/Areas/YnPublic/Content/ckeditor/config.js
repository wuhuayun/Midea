/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    // 界面语言，默认为 'en' 
    config.language = 'zh-cn';
    // 设置宽高
    //config.width = 550;
    config.height = 185;
    // 编辑器样式，有三种：'kama'（默认）、'office2003'、'v2'
    config.skin = 'v2';
    // 背景颜色
    config.uiColor = '#FFF';
    //config.toolbar = 'Basic';
    config.toolbar = 'Full';
    //工具栏的配置
    config.toolbar_Full = [
        ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'],  //,'Save'去掉保存按钮，这个已经被我注释掉
        //['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print','SpellChecker','Scayt'],
        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
        //['Form','Checkbox','Radio','TextField','Textarea','Select','Button',
        //'ImageButton','HiddenField'],也注释掉了
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        //['Link','Unlink','Anchor'],
        ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar',
        'PageBreak'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', 'ShowBlocks', '-', 'About']
    ];
    
    config.toolbarCanCollapse = true;
    /*
    //以下六行是上传路径的配置，这里是配置的aspx的页面，如果是在jsp，php，或者asp页面中，则需要把aspx换成相应的后缀名
    var ckfinderPath = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content"; //ckfinder路径
    config.filebrowserBrowseUrl = ckfinderPath + '/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = ckfinderPath + '/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = ckfinderPath + '/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = ckfinderPath + '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = ckfinderPath + '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = ckfinderPath + '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';*/
    config.filebrowserWindowWidth = '860';
    config.filebrowserWindowHeight = '640';
    
    //工具栏默认是否展开
    //config.toolbarStartupExpanded = false;
    // 当提交包含有此编辑器的表单时，是否自动更新元素内的数据
    config.autoUpdateElement = true;
};
