﻿var _product_function = {
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
            { id:'1',name:'Thương hiệu',type:3, attribute_id:1},
            { id: '2', name: 'Chất liệu', type: 3, attribute_id: 2 },
            { id: '3', name: 'Độ tuổi khuyến nghị', type: 3, attribute_id: 3 },
            { id: '4', name: 'Ngày sản xuất', type: 2, attribute_id: 4 },
            { id: '5', name: 'Tên tổ chức chịu trách nhiệm sản xuất', type: 3, attribute_id: 5 },
            { id: '6', name: 'Địa chỉ tổ chức chịu trách nghiệm sản xuất', type: 3, attribute_id: 6 },
            { id: '7', name: 'Sản phẩm đặt theo yêu cầu', type: 3, attribute_id: 7 },
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
                                        <div class="cat" style="display:none;">Phân loại hàng: {attribute}</div>

                                    </div>
                                </div>
                            </td>
                            <td class="text-center">{order_count}</td>
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
                           <td class="text-center">{order_count}</td>
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
                                    <b > Tên thuộc tính</b>
                                    <span class="mx-2" style="color: #919191; font-weight: normal;display:none; ">
                                        (Tùy
                                        chỉnh)
                                    </span>
                                    <nw class="edit-attributes-name-nw item "style="display:flex;" >
                                         <input type="text" class="form-control  attribute-name">
                                         <p type="error" style="display:none;"> </p>
                                        <a href="javascript:;" class="text-base edit-attributes-name-confirm"><i class="icofont-checked"></i></a>
                                        <a href="javascript:;" class="text-base edit-attributes-name-cancel" ><i class="icofont-close-squared-alt"></i></a>
                                    </nw>
                                    <a class="attribute-name-edit" style="display:none;" href="javascript:;"><i class="icofont-ui-edit"></i></a>
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
       
        ProductDetail_Specification_Row_Item: ` <div class="col-md-6 " >
                        <div class="item flex flex-lg-nowrap gap10 mb-2 w-100">
                            <label class="label">{name}<span style="display:none;">0/10</span></label>
                            <div class="wrap_input">
                               {wrap_input}

                            </div>
                        </div>
                    </div>`,
        ProductDetail_Specification_Row_Item_DateTime:`<div class="datepicker-wrap namesp" data-type="2" data-attr-id="{attribute_id}">
                                <input  placeholder="Vui lòng chọn"
                                       class="datepicker-input form-control" type="text" value="{value}">
                            </div>`,
        ProductDetail_Specification_Row_Item_Input: ` <div class="form-group namesp"data-type="3" data-attr-id="{attribute_id}">
                <input type="text" class="form-control" placeholder="{placeholder}" value="{value}">
                <a href="" class="edit"><i class="icofont-thin-down"></i></a>
           
            </div>`,
        ProductDetail_Specification_Row_Item_SelectOptions: ` <div class="form-group namesp"data-type="1" data-attr-id="{attribute_id}">
                <input type="text" class="form-control input-select-option" placeholder="{placeholder}" readonly value="{value}">
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
                            <li style=" list-style: none; "><input class="checkbox-option" type="checkbox" name="option1" value="option-1"> <span>Option 1</span></li>
                            <li style=" list-style: none; "><input class="checkbox-option" type="checkbox" name="option2" value="option-2"> <span>Option 2</span></li>
                            <li style=" list-style: none; "><input class="checkbox-option" type="checkbox" name="option3" value="option-3"> <span>Option 3</span></li>
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
        ProductDetail_Attribute_Price_Tr_Td:`<td class="td-attributes td-attribute-{i}" {row_span}>{name}</td>`,
        ProductDetail_Attribute_Price_TrMain:` <tr class="tr-main" data-attribute-1="Phân loại 1" data-attribute-2="Phân loại 2-1">
                                   {td_arrtibute}
                                    <td class="td-price">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control" placeholder="Giá nhập">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-profit">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control" placeholder="Lợi nhuận">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-amount">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control" placeholder="Giá bán">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-stock">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control" placeholder="Kho hàng">
                                        </div>
                                    </td>
                                    <td class="td-sku">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control" placeholder="SKU phân loại">
                                        </div>
                                    </td>
                                </tr>  `,
        ProductDetail_Attribute_Price_TrSub:`<tr class="tr-sub" data-attribute-1="Phân loại 1" data-attribute-2="Phân loại 2-2">
                                   {td_arrtibute}
                                    <td class="td-price">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Giá nhập">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-profit">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Lợi nhuận">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-amount">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Giá bán">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-stock">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control input-price" placeholder="Kho hàng">
                                        </div>
                                    </td>
                                    <td class="td-sku">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control" placeholder="SKU phân loại">
                                        </div>
                                    </td>
                                </tr>`,
        ProductDetail_DiscountGroupBuy_Row:` <tr class="discount-groupbuy-row">
                                    <td class="name">Khoảng giá 1</td>
                                    <td>
                                        <div class="flex gap10 flex-nowrap align-items-center justify-content-center">
                                            <div class="form-group mb-0 quanity-from">
                                                <input type="text" class="form-control input-price"
                                                       placeholder="Từ số lượng">
                                            </div>
                                            <i class="icofont-arrow-right"></i>
                                            <div class="form-group mb-0 quanity-to">
                                                <input type="text" class="form-control input-price"
                                                       placeholder="Đến số lượng">
                                            </div>
                                        </div>

                                    </td>
                                    <td>
                                        <div class="flex gap10 flex-nowrap align-items-center justify-content-center discount-value">
                                            <label class="radio mb-3">
                                                <input type="radio" name="{discount-type}" value="0">
                                                <span class="checkmark"></span>
                                            </label>
                                            <div class="form-group mb-0 price mr-3 discount-number">
                                                <input type="text" class="form-control input-price" placeholder="Nhập số">
                                                <span class="note">đ</span>
                                            </div>
                                            <label class="radio mb-3">
                                                <input type="radio" name="{discount-type}" value="1">
                                                <span class="checkmark"></span>

                                            </label>
                                            <div class="form-group mb-0 price discount-percent">
                                                <input type="text" class="form-control input-price" placeholder="Nhập số">
                                                <span class="note">%</span>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="text-center">
                                       <a href="javascript:;" class="delete-row">
                                            <i class="icofont-trash"></i>
                                       </a>
                                    </td>

                                </tr>`,
        ProductDetail_GroupProduct_ResultDirection:`<b>{name}<i class="icofont-thin-right"></i></b>`,
        ProductDetail_GroupProduct_ResultSelected:`<b >{name}</b>`,
        ProductDetail_GroupProduct_colmd4_Li:` <li data-id="{id}" data-name="{name}"><a href="javascript:;">{name}<i class="{icofont-thin-right}"></i></a></li>`,
        ProductDetail_GroupProduct_colmd4:`<div class="col-md-4" data-level="{level}">
                        <div class="list-toys">
                            <h6><a href="">{name}<i class="icofont-thin-right"></i></a></h6>
                            <ul>
                               {li}
                                
                            </ul>
                        </div>
                    </div>`
    }
}