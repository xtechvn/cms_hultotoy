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
        ProductDetail_Max_Image: 9,
        DefaultSpecificationValue: [
            { id:'1',name:'Thương hiệu',type:1},
            { id: '2', name: 'Chất liệu', type: 1 },
            { id: '3', name: 'Độ tuổi khuyến nghị', type: 1 },
            { id: '4', name: 'Ngày sản xuất', type: 2 },
            { id: '5', name: 'Tên tổ chức chịu trách nhiệm sản xuất', type: 1 },
            { id: '6', name: 'Địa chỉ tổ chức chịu trách nghiệm sản xuất', type: 1 },
            { id: '7', name: 'Sản phẩm đặt theo yêu cầu', type: 1 },
        ]
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
                                    <input class="image_input" type="file" name="myFile">
                                    <div class="choose-content choose-product-images">
                                        <i class="icofont-image"></i>
                                        <span>Thêm hình ảnh (<nw class="count">{0}</nw>/{max})</span>
                                    </div>
                                </label>
                            </div>`,
        ProductDetail_Images_Item:` <div class="items magnific_popup" data-src="{src}" data-id="{id}">
                                <button type="button" class="delete"><i class="icofont-close-line"></i></button>
                                <a class="thumb_img thumb_1x1 magnific_thumb">
                                    <img src="{src}">
                                </a>
                            </div>`,
        ProductDetail_Attribute_Row_Item:` <div class="col-md-6 lastest-attribute-value item">
                            <div class="box-list">
                                <div class="form-group namesp flex-input-choose">
                                    <label class="choose choose-wrap">
                                        <input type="file" name="myFile">
                                        <div class="choose-content">
                                            <i class="icofont-image"></i>
                                        </div>
                                    </label>
                                    <div class="relative w-100">
                                        <input type="text" class="form-control attributes-name" placeholder="Tên thuộc tính" maxlength="14" value="">
                                        <p class="error" style="display:none;"> </p>
                                        <span class="note"><nw class="count">0</nw>/14</span>
                                    </div>
                                    <div class="right-action">
                                        <a class="icon-action attribute-item-delete" style="display:none;" href="javascript:;"><i class="icofont-trash"></i></a>
                                        <a class="icon-action attribute-item-add" href="javascript:;"><i class="icofont-drag"></i></a>
                                    </div>

                                </div>
                            </div>
                        </div>`,

        ProductDetail_Attribute_Row_Add_Attributes:` <div class="item-edit item flex flex-lg-nowrap gap10 mb-2 w-100 product-attributes-add">
                <label class="label"></label>
                <div class="wrap_input pb-4">
                    <button  class="btn btn-add btn-add-attributes"><i class="icofont-plus"></i>Thêm phân loại</button>
                </div>
            </div>`,
        ProductDetail_Attribute_Row: ` <div class="item-edit item flex flex-lg-nowrap gap10 mb-2 w-100 product-attributes" >
                <label class="label">Phân loại hàng</label>
                <div class="wrap_input pb-4">
                    <span class="delete">
                        <i class="icofont-close"></i>
                    </span>
                    <div class="row ">
                        <div class="col-md-6">
                            <div class="box-list">
                                <h6>
                                    <b> Tên thuộc tính</b>
                                    <span class="mx-2" style="color: #919191; font-weight: normal;">
                                        (Tùy
                                        chỉnh)
                                    </span>
                                    <nw id="edit-attributes-name-nw item" style="display:none;">
                                         <input type="text" class="attribute-name">
                                         <p type="error" style="display:none;"> </p>
                                        <a href="javascript:;" class="text-base edit-attributes-name-confirm"><i class="icofont-checked"></i></a>
                                        <a href="javascript:;" class="text-base edit-attributes-name-cancel" ><i class="icofont-close-squared-alt"></i></a>
                                    </nw>
                                    <a class="attribute-name-edit" href="javascript:;"><i class="icofont-ui-edit"></i></a>
                                </h6>

                            </div>
                        </div>
                    </div>
                    <div class="line-bottom"></div>
                    <div class="row row-attributes-value">
                       {html}
                    </div>
                </div>
            </div>`,
       
        ProductDetail_Specification_Row_Item: ` <div class="col-md-6 " data-type="{type}">
                        <div class="item flex flex-lg-nowrap gap10 mb-2 w-100">
                            <label class="label">{name}<span style="display:none;">0/10</span></label>
                            <div class="wrap_input">
                               {wrap_input}

                            </div>
                        </div>
                    </div>`,
        ProductDetail_Specification_Row_Item_DateTime:`<div class="datepicker-wrap">
                                <input id="datepicker" placeholder="Vui lòng chọn"
                                       class="datepicker-input form-control" type="text" value="">
                            </div>`,
        ProductDetail_Specification_Row_Item_SelectOptions: ` <div class="form-group namesp">
                <input type="text" class="form-control" placeholder="{placeholder}" >
                <a href="" class="edit"><i class="icofont-thin-down"></i></a>
            </div>
            <div class="select-option p-2" style="width: 70%;display:none;">
                <div class="them-chatlieu">
                    <div class="content_lightbox">
                        <div class="form-group w-100 ">
                            <div class="search-wrapper">
                                <input type="text" class="input_search onclick-togle border" name=""
                                       value="" placeholder="Chọn giá trị ...">
                                <span class="search-btn">
                                    <button type="submit">
                                        <svg class="icon-svg">
                                            <use xlink:href="/images/icons/icon.svg#search"></use>
                                        </svg>
                                    </button>
                                </span>
                            </div>
                        </div>
                        <ul>
                            <li style=" list-style: none; "><input type="checkbox" name="option1" value="1"> <span>Option 1</span></li>
                            <li style=" list-style: none; "><input type="checkbox" name="option2" value="1"> <span>Option 2</span></li>
                            <li style=" list-style: none; "><input type="checkbox" name="option3" value="1"> <span>Option 3</span></li>
                        </ul>
                        <div class="border-top text-center pt-2">
                            <a href="javascript:;" class="text-primary add-specificaion-value">
                                <i class="icofont-plus mr-2"></i>Thêm thuộc
                                tính mới
                            </a>
                            <div class="flex flex-nowrap gap10 align-items-center add-specificaion-value-box" style="display:none;">
                                <input type="text" class="form-control" placeholder="Nhập vào">
                                <a href="javascript:;" class="text-base add-specificaion-value-add" style="font-size: 18px">
                                    <i class="icofont-checked"></i>
                                </a>
                                <a href="javascript:;" class="add-specificaion-value-cancel" style="font-size: 18px;">
                                    <i class="icofont-close-squared-alt "></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`,
    }
}