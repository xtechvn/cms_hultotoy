$(document).ready(function () {
    product_detail.Initialization()
})
var product_detail = {
    Initialization: function () {
        var model = [{ url: '/', name: 'Trang chủ' }, { url: '/productv2', name: 'Quản lý sản phẩm' }, { url: 'javascript:;', name: 'Chi tiết sản phẩm', activated: true }]
        _global_function.RenderBreadcumb(model)
        $('.add-product').addClass('placeholder')
        $('.add-product').addClass('box-placeholder')

        product_detail.HideProductTab()
        product_detail.Detail()
        product_detail.DynamicBind()

        $('.select-group-product').magnificPopup({
            type: 'inline',
            midClick: true,
            closeOnBgClick: false, // tránh đóng khi người dùng nhấn vào vùng ngoài
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            preloader: false,
            removalDelay: 300,
        });
    },
    DynamicBind: function () {
        $('body').on('click', '.choose-product-images', function () {
            var element = $(this)
            if (product_detail.GetImagesCount() >= _product_constants.VALUES.ProductDetail_Max_Image) {
                _msgalert.error('Số lượng ảnh sản phẩm không được vượt quá ' + _product_function.Comma(_product_constants.VALUES.ProductDetail_Max_Image) + ' ảnh')
            }
            else {
                element.closest('div').find('input').trigger('click')
            }
        });
        $('body').on('change', '.image_input', function () {
            var element = $(this)
            product_detail.AddProductImages(element)


        });
        $('body').on('click', '.select-group-product', function () {
            var element = $(this)


        });
        $('body').on('keyup', '#description textarea, #product-name input, .product-attributes input', function () {
            var element = $(this)
            element.closest('.item').find('.count').html(_product_function.Comma(element.val().length))

        });
        $('body').on('click', '.btn-cancel', function () {
            var element = $(this)
            product_detail.Cancel()

        });
        $('body').on('click', '.btn-hide', function () {
            var element = $(this)
            product_detail.Hide()

        });
        $('body').on('click', '.btn-update', function () {
            var element = $(this)
            product_detail.Summit()

        });
        $('body').on('click', '.item-edit .delete', function () {
            var element = $(this)
            element.closest('.product-attributes').next().remove()
            element.closest('.product-attributes').remove()

        });
        $('body').on('click', '.btn-add-attributes', function () {
            var element = $(this)
            product_detail.AddProductAttributes()

        });
        $('body').on('keyup', '.lastest-attribute-value .attributes-name', function () {
            var element = $(this)
            input_element.closest('.relative').find('.error').hide()
            product_detail.ValidateAttributesInput(element)
            element.closest('.lastest-attribute-value').removeClass('lastest-attribute-value')
            if (element.val() != undefined && element.val().trim() != '') {
                element.closest('.row-attributes-value').append(_product_constants.HTML.ProductDetail_Attribute_Row_Item)
                $('.row-attributes-value .col-md-6 .attribute-item-delete').show()
                if (element.closest('.row-attributes-value').find('.col-md-6').length < 2) {
                    element.closest('.row-attributes-value').find('.attribute-item-delete').hide()
                }
            }
        });
        $('body').on('click', '.attribute-item-delete', function () {
            var element = $(this)
            if (element.closest('.row-attributes-value').find('.col-md-6').length <= 2) {
                element.closest('.row-attributes-value').find('.attribute-item-delete').hide()
            }
            element.closest('.col-md-6').remove()
           

        });
        $('body').on('click', '.attribute-item-add', function () {
            var element = $(this)
            element.closest('.row-attributes-value').append(_product_constants.HTML.ProductDetail_Attribute_Row_Item)
            element.closest('.row-attributes-value').find('.attribute-item-delete').show()

        });
        $('body').on('click', '.attribute-name-edit', function () {
            var element = $(this)
            element.hide()
            $('#edit-attributes-name-nw').show()
            element.closest('h6').find('b').attr('data-value', element.closest('h6').find('b').html())
            element.closest('h6').find('b').html('Nhập tên thuộc tính mới')
            element.closest('h6').find('span').hide()
            element.hide()

        });
        $('body').on('click', '.edit-attributes-name-confirm', function () {
            var element = $(this)
            var value = element.closest('h6').find('.attribute-name').val()
            element.closest('h6').find('b').html(value)
            $('#edit-attributes-name-nw').hide()
            element.closest('h6').find('.attribute-name-edit').show()
            element.closest('h6').find('span').show()
        });
        $('body').on('click', '.edit-attributes-name-cancel', function () {
            var element = $(this)
            $('#edit-attributes-name-nw').hide()
            element.closest('h6').find('b').html(element.closest('h6').find('b').attr('data-value'))
            element.closest('h6').find('.attribute-name-edit').show()
            element.closest('h6').find('span').show()

        });
        $('body').on('click', '.specifications-box .col-md-6 .namesp input', function () {
            var element = $(this)
            $('.specifications-box .col-md-6 .select-option').fadeOut()
            if (element.closest('.col-md-6').find('.select-option').length > 0) {
                element.closest('.col-md-6').find('.select-option').fadeIn()
            }

        });
        //-- Click outside div global function:
        $('body').on('click', function (e) {
            var location = $(e.target)
            if (location.closest('.specifications-box').length === 0) {
                $('.specifications-box .col-md-6 .select-option').fadeOut()
            }
            else if (location.closest('.col-md-6').find('.select-option').length === 0) {
                $('.specifications-box .col-md-6 .select-option').fadeOut()
            }



        });
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value', function (e) {
            var element = $(this)
            element.closest('.border-top').find('.add-specificaion-value-box').show()
            element.hide()
         
        });
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value-add ,.specifications-box .col-md-6 .add-specificaion-value-cancel ', function (e) {
            var element = $(this)
            element.closest('.border-top').find('.add-specificaion-value-box').hide()
            element.closest('.border-top').find('.add-specificaion-value').show()

        });
        //$('body').on('click', '.select-group-product', function (e) {
        //    var element = $(this)
        //    $.magnificPopup.open({
        //        items: {
        //            src: '#them-nganhhang'
        //        },
        //        type: 'inline',
        //        midClick: true,
        //        callbacks: {
        //            beforeOpen: function () {
        //                this.st.mainClass = currentPopup.st.mainClass;
        //            }
        //        }
        //    });
        //});
    },
    Detail: function () {
        var product_id = $('#product_detail').val()
        if (product_id != null && product_id != undefined && product_id.trim() != '') {
            _product_function.POST('/Product/ProductDetail', { product_id: $('#product_detail').val() }, function (result) {
                if (result.is_success && result.data) {
                    product_detail.RenderGeneralFunction()
                    product_detail.RenderExistsProduct(result.data)
                }
                else {
                    window.location.href = '/error'
                }
            });
        }
        else {
            product_detail.RenderGeneralFunction()
            product_detail.RenderAddNewProduct()
            $('.btn-update').html('Thêm mới')
        }


    },

    RenderGeneralFunction: function () {
        $('#images .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', _product_constants.VALUES.ProductDetail_Max_Image))
        $('#avatar .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list span').html('Thêm video (<nw class="count">0</nw>/1)')
        product_detail.ShowProductTab()
        $('.add-product').removeClass('placeholder')
        $('.add-product').removeClass('box-placeholder')
    },
    RenderAddNewProduct: function () {
        $('#product-attributes-box').html('')
        var html = ''
        $(_product_constants.VALUES.DefaultSpecificationValue).each(function (index, item) {
            switch (item.type) {
                case 2: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_DateTime

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)

                } break;
                default: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)

                } break;
            }
        })
        $('.specifications-box').html(html)
    },
    RenderExistsProduct: function (product) {
        $('#product_detail').attr('data-code', product.code)

    },
    GetImagesCount: function () {
        return (($('#images .list .items').length) - 1);
    },
    ShowProductTab: function () {
        $('#specification-disabled').hide()
        $('#selling-information-disabled').hide()
        $('#other-information-disabled').hide()

        $('#specifications').show()
        $('#selling-information').show()
        $('#other-information').show()
    },
    HideProductTab: function () {
        $('#specification-disabled').show()
        $('#selling-information-disabled').show()
        $('#other-information-disabled').show()

        $('#specifications').hide()
        $('#selling-information').hide()
        $('#other-information').hide()
    },
    Cancel: function () {
        let title = 'Xác nhận hủy sản phẩm';
        let description = 'Bạn có chắc chắn muốn hủy chỉnh sửa/ thêm mới sản phẩm này?';

        _msgconfirm.openDialog(title, description, function () {
            window.location.href = '/Product'

        });
    },
    Hide: function () {
        let title = 'Xác nhận ẩn sản phẩm';
        let description = 'Bạn có chắc chắn muốn ẩn sản phẩm này?';

        _msgconfirm.openDialog(title, description, function () {
            window.location.href = '/Product'

        });
    },
    AddProductImages: function (element) {
        $(element[0].files).each(function (index, item) {

            var reader = new FileReader();
            reader.onload = function (e) {
                element.closest('.list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', e.target.result).replaceAll('{id}', '-1'))
            }
            reader.readAsDataURL(item);

        });
        element.val(null)
    },
    Summit: function () {
        var model = {
            _id: $('#product_detail').val() == undefined || $('#product_detail').val().trim() == '' ? null : $('#product_detail').val(),
            code: $('#product_detail').attr('data-code') == undefined || $('#product_detail').attr('data-code').trim() == '' ? null : $('#product_detail').attr('data-code'),
            amount: parseFloat($('#main-amount input').val().replaceAll(',','')),
            discount: 0,
            quanity_of_stock: parseInt($('#stock input').val().replaceAll(',', ''))
        }
        model.images =[]
        $('#images .list .items').each(function (index, item) {
            var element_image = $(this)
            model.images.push(element_image.find('img').attr('src'))
        })
        model.avatar = $('#avatar .list .items').first().find('img').attr('src')
        model.videos = []
        $('#videos .list .items').each(function (index, item) {
            var element_image = $(this)
            model.images.push(element_image.find('img').attr('src'))
        })
        model.name = $('#product-name input').val()
        model.group_product_id = $('#group-id input').attr('data-id')
        model.description = $('#description textarea').val()
        model.specification = []

        model.attributes = []
        model.discount_group_buy = []
        model.preorder_status = $('input[name="preorder_status"]') == 1 ? true : false
        model.condition_of_product = $('#condition_of_product').find(':selected').val()
        model.sku = $('#sku input').val()
    },
   
    AddProductAttributes: function () {
        var attribute_max_count = 2
        if ($('.product-attributes').length < attribute_max_count) {
            $('#product-attributes-box').append(_product_constants.HTML.ProductDetail_Attribute_Row.replaceAll('{html}', _product_constants.HTML.ProductDetail_Attribute_Row_Item))
            if ($('.product-attributes').length < attribute_max_count) {
                $('#product-attributes-box').append(_product_constants.HTML.ProductDetail_Attribute_Row_Add_Attributes)
            }
        }
      
    },
    ValidateProduct: function (model) {
        var success = true;


        return success
    },
    ValidateAttributesInput: function (element) {
        var success = true;
        element.closest('.row-attributes-value').find('.attributes-name').each(function (index, item) {
            var input_element = $(this)
            if (input_element.val() == undefined || input_element.val().trim() == '') {
                if (success) success = false
                input_element.closest('.relative').find('.error').show()
                input_element.closest('.relative').find('.error').html('Không được để trống ô')
            }
        })
        return success
    }
}