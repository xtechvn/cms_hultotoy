var _product_function = {
    POST: function (url,model,callback) {
        $.ajax({
            url: url,
            type: "POST",
            data: model,
            success: function (result) {
                callback(result)
            },
            error: function (err) {
                console.log(err)
            }
        });
    },
    Comma: function (number) { //function to add commas to textboxes
        number = ('' + number).replace(/[^0-9.,]+/g, '');
        number += '';
        number = number.replaceAll(',', '');
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        return x1 + x2;
    },
}
var _product_constants = {
    VALUES: {
        ProductDetail_Max_Image:9,
    },
    HTML: {
        Product: `
            <tr data-id="{id}">
                            <td style="width: 0;">
                                <label class="check-list mb-3">
                                    <input type="checkbox">
                                    <span class="checkmark"></span>
                                </label>
                            </td>
                            <td style="width: 100px;">
                                <div class="item-order">
                                    <div class="img">
                                        <img src="{avatar}" alt="">
                                    </div>
                                    <div class="info">
                                        <h3 class="name-product">
                                           {name}
                                        </h3>
                                        <div class="cat">Phân loại hàng: {attribute}</div>

                                    </div>
                                </div>
                            </td>
                            <td class="text-center">{amount}</td>
                            <td class="text-center">{amount}</td>
                            <td class="text-center">{stock}</td>
                            <td class="text-center">
                                <a href="javascript:;" class="product-edit">Cập nhật</a><br />
                                <a href="javascript:;" class="product-viewmore">Xem thêm</a>
                            </td>
                        </tr>
        `,
        SubProduct:`  <tr class="sub-product"data-id="{id}"data-main-id="{main_id}">
                            <td style="width: 0;">
                            </td>
                            <td style="width: 100px;">
                                <div class="item-order">
                                    <div class="img">
                                        <img src="{avatar}" alt="">
                                    </div>
                                    <div class="info">
                                        <h3 class="name-product"> {attribute}</h3>
                                        <div class="cat">{sku}</div>
                                    </div>
                                </div>
                            </td>
                           <td class="text-center">{amount}</td>
                            <td class="text-center">{amount}</td>
                            <td class="text-center">{stock}</td>
                            <td class="text-center"></td>
                        </tr>`,
        ProductDetail_Images_AddImagesButton:`<div class="items import">
                                <label class="choose choose-wrap">
                                    <input type="file" name="myFile">
                                    <div class="choose-content choose-product-images">
                                        <i class="icofont-image"></i>
                                        <span>Thêm hình ảnh (<nw class="count">{0}</nw>/{max})</span>
                                    </div>
                                </label>
                            </div>`,
        ProductDetail_Images_Item:`<div class="items magnific_popup">
                                <button type="button" class="delete"><i class="icofont-close-line"></i></button>
                                <a class="thumb_img thumb_1x1 magnific_thumb">
                                    <img src="{src}">
                                </a>
                            </div>`
    }
}