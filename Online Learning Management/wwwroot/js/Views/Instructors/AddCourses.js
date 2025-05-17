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

let defaultImage = null;

function OnStart() {
    let id = $('#inputId').val();
    const img = $('#previewImage');

    if (id != 0) {
        defaultImage = img.attr('src');
        console.log(defaultImage);
    } else {
        defaultImage = null;
        img.hide();
    }
}

function previewSelectedImage(input) {
    let id = $('#inputId').val();
    const img = $('#previewImage');

    // No file selected, fallback to original
    if (!input.files || input.files.length === 0) {
        if (id != 0 && defaultImage) {
            img.attr('src', defaultImage).show();
        } else {
            img.hide();
        }
        return;
    }

    const file = input.files[0];
    const fileName = file.name.toLowerCase();
    const validExtensions = ['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'];

    const fileExtension = fileName.split('.').pop();

    if (!validExtensions.includes(fileExtension)) {
        img.hide();
        return;
    }

    if (id == 0) {
        img.show();
    }

    // File selected – preview it
    const reader = new FileReader();
    reader.onload = function (e) {
        img.attr('src', e.target.result);
    };
    reader.readAsDataURL(input.files[0]);
}
