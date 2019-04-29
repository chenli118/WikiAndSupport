// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

(function ($) {
    function HomeIndex() {
        var $this = this;

        function initialize() {
            if (window.location.href.indexOf("NewIssue") < 0 && window.location.href.indexOf("NewAnswer") < 0) return;            
            $('#Issue_Content').summernote({
                focus: true,
                height: 150,
                codemirror: {
                    theme: 'united'
                }
            });
            $('#Answer_Text').summernote({ 
                focus: true,
                height: 150,
                codemirror: {
                    theme: 'united'
                },
                callbacks: {
                    onChange: function (contents, $editable) {
                        $('#Answer_Text').val(contents);
                    }
                }

            });            
            var isubmit = document.getElementById('Issue_Submit');           
            isubmit.addEventListener('click', iv);            
        }
        function iv() {
            var v = $('#Issue_Content').summernote('code');           
            $('#Issue_Content').val(v);
        }
        $this.init = function () {
            initialize();
        }
    }
    $(function () {
        var self = new HomeIndex();
        self.init();
         
    })
}(jQuery)) 