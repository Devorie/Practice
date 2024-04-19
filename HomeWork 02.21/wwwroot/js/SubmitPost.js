$(() => {

    $("#title, #content").on('input', function () {
        console.log('hello')
        ensureFormValidity()
    })

    function ensureFormValidity() {
        console.log('hi');
        const title = $("#title").val();
        const content = $("#content").val();
        const isValid = title && content;
        $('#submit-button').prop('disabled', !isValid);
    }

})