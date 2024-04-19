$(() => {

    $("#commenter, #content").on('input', function () {
        console.log('hello')
        ensureFormValidity()
    })

    function ensureFormValidity() {
        console.log('hi');
        const commenter = $("#commenter").val();
        const content = $("#content").val();
        console.log(commenter);
        const isValid = commenter && content;
        console.log(!!isValid);
        $('#submit').prop('disabled', !isValid);
    }

})