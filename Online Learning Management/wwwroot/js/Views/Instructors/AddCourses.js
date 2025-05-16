// ✅ Custom validator: StartDate >= Today
$.validator.addMethod("datenotinpast", function (value, element) {
    if (!value) return true;
    var today = new Date();
    today.setHours(0, 0, 0, 0);
    var inputDate = new Date(value);
    return inputDate >= today;
});

$.validator.unobtrusive.adapters.add("datenotinpast", [], function (options) {
    options.rules["datenotinpast"] = true;
    options.messages["datenotinpast"] = options.message;
});

// ✅ Custom validator: EndDate > StartDate
$.validator.addMethod("dategreaterthan", function (value, element, params) {
    var other = $("[name='" + params.otherproperty + "']").val();
    if (!value || !other) return true;
    return new Date(value) > new Date(other);
});

$.validator.unobtrusive.adapters.add("dategreaterthan", ["otherproperty"], function (options) {
    options.rules["dategreaterthan"] = { otherproperty: options.params.otherproperty };
    options.messages["dategreaterthan"] = options.message;
});

$.validator.addMethod("filerequired", function (value, element) {
    let id = $('#inputId').val();
    if (isNaN(id)) return true;
    if (id == 0) {
        if (value && value.length > 0) {
            const allowedExtensions = ['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'];
            const fileExtension = value.split('.').pop().toLowerCase();

            return allowedExtensions.includes(fileExtension);
        }
        return false;
    }
    return true;
});

$.validator.unobtrusive.adapters.add("filerequired", ["dependentproperty"], function (options) {
    options.rules["filerequired"] = { dependentproperty: options.params.dependentproperty };
    options.messages["filerequired"] = options.message;
});