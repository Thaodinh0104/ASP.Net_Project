tinymce.init({
    selector: "textarea",
    width: '100%',
    height: '300',
    browser_spellcheck: true,
    paste_data_images: true,
    images_upload_url: '../../UploadAjax/uploadImage',
    images_reuse_filename: true,
    file_picker_types: 'image',
    relative_urls: false,
    remove_script_host: false,
    plugins: [
        "advlist autolink autosave link image lists charmap print preview hr anchor pagebreak spellchecker",
        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
        "table contextmenu directionality template textcolor paste fullpage textcolor colorpicker textpattern"
    ],
    toolbar1: "bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | formatselect fontselect fontsizeselect",
    toolbar2: "cut copy paste | searchreplace | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor image code | insertdatetime preview | forecolor backcolor",
    toolbar3: "table | hr removeformat | subscript superscript | charmap emoticons | print fullscreen | ltr rtl | spellchecker | visualchars visualblocks nonbreaking template pagebreak restoredraft",
    menubar: false,
    image_advtab: true,
    toolbar_items_size: 'small',
    file_picker_callback: function (callback, value, meta) {
        if (meta.filetype === 'image') {
            var inputFile = document.createElement("INPUT");
            inputFile.setAttribute("type", "file");
            inputFile.setAttribute("style", "display: none");
            inputFile.click();
            inputFile.addEventListener("change", function () {
                var file = this.files[0];
                var reader = new FileReader();
                reader.onload = function (e) {
                    callback(e.target.result, {
                        alt: ''
                    });
                };
                reader.readAsDataURL(file);
            });
        }
    },
    insertdatetime_dateformat: "%d/%m/%Y",
    insertdatetime_timeformat: "%H:%M:%S",
});