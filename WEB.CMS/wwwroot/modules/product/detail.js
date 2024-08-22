﻿
$(document).ready(function () {
    product_detail.Initialization()
})
var product_detail = {
    Initialization: function () {
        var model = [{ url: '/', name: 'Trang chủ' }, { url: '/product', name: 'Quản lý sản phẩm' }, { url: 'javascript:;', name: 'Chi tiết sản phẩm', activated: true }]
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
        $('body').on('click', '.change-tab', function () {
            var element = $(this)
            switch (element.attr('data-id')) {
                case '1': {
                    $("#images").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
                case '2': {
                    $("#selling-information").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
                case '3': {
                    $("#other-information").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
                case '4': {
                    $("#other-information").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
            }
        });
        $('body').on('click', '.magnific_popup .delete', function () {
            var element = $(this)
            element.closest('.items').remove()
        });
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
      
        $('body').on('click', '.item-edit .delete', function () {
            var element = $(this)
            element.closest('.product-attributes').next().remove()
            element.closest('.product-attributes').remove()
            if ($('.product-attributes').length <= 0) {
                $('#single-product-amount').show()
                $('#product-attributes-price').hide()
                $('#product-attributes-all-price').closest('.item-edit').hide()

            }
        });
        $('body').on('click', '.btn-add-attributes', function () {
            var element = $(this)
            product_detail.AddProductAttributes()
            product_detail.RenderRowAttributeTablePrice()
            $('#single-product-amount').hide()
            $('#product-attributes-price').show()
            $('#product-attributes-all-price').closest('.item-edit').show()

        });
        
        $('body').on('keyup', '.lastest-attribute-value .attributes-name', function () {
            var element = $(this)
            element.closest('.relative').find('.error').hide()
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
            element.closest('h6').find('b').attr('data-value', element.closest('h6').find('b').html())
            element.closest('h6').find('b').html('Nhập tên thuộc tính mới')
            element.closest('h6').find('b').hide()
            element.closest('h6').find('span').hide()
            element.closest('h6').find('.edit-attributes-name-nw').show()
            element.hide()

        });
        $('body').on('keyup', '.edit-attributes-name-nw input', function () {
            var element = $(this)
            var value = element.closest('h6').find('.attribute-name').val()
            element.closest('h6').find('b').html(value)

        });
        $('body').on('click', '.edit-attributes-name-confirm', function () {
            var element = $(this)
            var value = element.closest('h6').find('.attribute-name').val()
            element.closest('h6').find('b').html(value)
            element.closest('h6').find('b').show()

            element.closest('h6').find('.edit-attributes-name-nw').hide()
            element.closest('h6').find('.attribute-name-edit').show()
            element.closest('h6').find('span').show()
        });
        $('body').on('click', '.edit-attributes-name-cancel', function () {
            var element = $(this)
            element.closest('h6').find('b').html(element.closest('h6').find('b').attr('data-value'))
            element.closest('h6').find('.edit-attributes-name-nw').hide()
            element.closest('h6').find('b').show()
            element.closest('h6').find('span').show()

        });
        $('body').on('keyup', '#specifications .them-chatlieu .input_search', function () {
            var element = $(this)
            setTimeout(function () {
                product_detail.RenderSpecificationLi(element)
            }, 1000);
        });
        $('body').on('click', '.specifications-box .col-md-6 .namesp input', function () {
            var element = $(this)
            var parent = element.closest('.col-md-6').find('.select-option')
            $('.specifications-box .col-md-6 .select-option').each(function (index, item) {
                var compare = $(this)
                if (compare.is(parent)) {
                    if (parent.is(':hidden')) {
                        parent.fadeIn()
                        product_detail.RenderSpecificationLi(compare)
                        
                    } else {
                        parent.fadeOut()
                    }
                } else {
                    compare.fadeOut()
                }


            })
            //$('.specifications-box .col-md-6 .select-option').fadeOut()

            //if (element.closest('.col-md-6').find('.select-option').length > 0 && element.closest('.col-md-6').find('.select-option').is('hidden')) {
            //    element.closest('.col-md-6').find('.select-option').fadeIn()
            //}

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
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value-add , .specifications-box .col-md-6 .add-specificaion-value-cancel ', function (e) {
            var element = $(this)
            element.closest('.border-top').find('.add-specificaion-value-box').hide()
            element.closest('.border-top').find('.add-specificaion-value').show()

        });
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value-add ', function (e) {
            var element = $(this)
            var parent = element.closest('.them-chatlieu').find('ul')
            var name = element.closest('.add-specificaion-value-box').find('input').val()
            parent.prepend(_product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions_NewOptions
                .replaceAll('{checked}', 'checked')
                .replaceAll('{value}', 'undefined') 
                .replaceAll('{name}', name) 
            )
            element.closest('.add-specificaion-value-box').find('input').val('')
            product_detail.RenderSpecificationSelectOption(element.closest('.them-chatlieu').find('ul').find('li'))
            var type = element.closest('.col-md-6').find('.namesp').attr('data-attr-id')
            
            _product_function.POST('/Product/AddProductSpecification', { type: type, name: name }, function (result) {
               
            });
        });
     
        $('body').on('click', '.btn-add-attributes, .edit-attributes-name-confirm, .attribute-item-add, .attribute-item-delete, #product-attributes-box .delete', function () {
            product_detail.RenderRowAttributeTablePrice()

        });
        $('body').on('focusout', '.attributes-name', function () {
            product_detail.RenderRowAttributeTablePrice()

        });
        $('body').on('click', '.btn-all', function () {
            product_detail.ApplyAllPriceToTable()


        });
        $('body').on('click', '.btn-add-discount-groupbuy', function () {
            $('#discount-groupbuy').show()
            $('.btn-add-discount-groupbuy').closest('.row').hide()
            var id = $('.discount-groupbuy-row').length + 1
            var html = _product_constants.HTML.ProductDetail_DiscountGroupBuy_Row
                .replaceAll('Khoảng giá 1', 'Khoảng giá ' + id)
                .replaceAll('{discount-type}', 'discount-type-' + id)
                
            $(html).insertBefore('#discount-groupbuy .summary')
        });
        $('body').on('click', '#discount-groupbuy .delete-row', function () {
            var element = $(this)
            element.closest('.discount-groupbuy-row').remove()
            if ($('.discount-groupbuy-row').length <=0) {
                $('#discount-groupbuy').hide()
                $('.btn-add-discount-groupbuy').closest('.row').show()
            }
                     
        });
        $('body').on('keyup', '.input-price', function () {
            var element = $(this)
            var value = element.val()
            element.val(Comma(value))
            product_detail.UpdateRowData(element.closest('tr'))
        });
        $('body').on('click', '#discount-groupbuy .summary .btn-add', function () {

            var id = $('.discount-groupbuy-row').length + 1
            var html = _product_constants.HTML.ProductDetail_DiscountGroupBuy_Row
                .replaceAll('Khoảng giá 1', 'Khoảng giá ' + id)
                .replaceAll('{discount-type}', 'discount-type-' + id)

            $(html).insertBefore('#discount-groupbuy .summary')

        });
        $('body').on('click', '#them-nganhhang .col-md-4 li', function () {
            var element = $(this)
            element.closest('.col-md-4').find('li').removeClass('active')
            element.addClass('active')
            product_detail.RenderOnSelectGroupProduct(element)
        });
        $('body').on('click', '.action .btn-outline-base', function () {
            $.magnificPopup.close()
        });
        $('body').on('click', '.action .btn-round', function () {
            product_detail.RenderSelectedGroupProduct()
            $.magnificPopup.close()
        });
        $('body').on('click', '#product-detail-cancel', function () {
            let title = 'Xác nhận hủy';
            let description = 'Dữ liệu đã chỉnh sửa sẽ không được lưu, bạn có chắc chắn không?';
            _msgconfirm.openDialog(title, description, function () {
                window.location.href('/product/detail')

            });
        });
        $('body').on('click', '#product-detail-cancel', function () {
            let title = 'Xác nhận ẩn sản phẩm';
            let description = 'Sản phẩm sẽ không còn được hiển thị ngoài trang sản phẩm, bạn có chắc chắn không?';
            _msgconfirm.openDialog(title, description, function () {
                _product_function.POST('/Product/CancelProduct', { product_id: $('#product_detail').val() }, function (result) {
                    if (result.is_success && result.data) {
                        _msgalert.success('Ẩn sản phẩm thành công')
                        setTimeout(function () {
                            window.location.href('/product/detail')
                        }, 2000);
                    }
                    else {
                    }
                });

            });
        });
        $('body').on('click', '#product-detail-confirm', function () {
            product_detail.Summit()
        });
        $('body').on('click', '.them-chatlieu .checkbox-option', function () {
            var element = $(this)

            product_detail.RenderSpecificationSelectOption(element)

        });
        $('body').on('keyup', '.col-md-6 .input-select-option', function (e) {
            e.preventDefault()
        });
    },
    Detail: function () {
        var product_id = $('#product_detail').val()
        if (product_id != null && product_id != undefined && product_id.trim() != '') {
            _product_function.POST('/Product/ProductDetail', { product_id: $('#product_detail').val() }, function (result) {
                if (result.is_success && result.data) {
                    product_detail.RenderExistsProduct(result.data)

                }
                else {
                    window.location.href = '/error'
                }
            });
        }
        else {
            $('.btn-update').html('Thêm mới')
            product_detail.RenderAddNewProduct()

        }


    },

    RenderAddNewProduct: function () {
        $('#product-attributes-box').html('')
        $('#discount-groupbuy').hide()
        $('.btn-add-discount-groupbuy').closest('.row').show()

        var html = ''
        $(_product_constants.VALUES.DefaultSpecificationValue).each(function (index, item) {
            switch (item.type) {
                case 1: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_Input
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)

                        .replaceAll('{value}', '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type)
                        .replaceAll('{name}', item.name)
                        .replaceAll('{wrap_input}', html_item)
                      

                } break;
                case 2: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_DateTime
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)

                        .replaceAll('{value}', '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)


                } break;
                default: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)

                        .replaceAll('{value}', '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)
                        



                } break;
            }
        })
        $('.specifications-box').html(html)
        product_detail.RenderRowAttributeTablePrice()
        $('#images .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', _product_constants.VALUES.ProductDetail_Max_Image))
        $('#avatar .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list span').html('Thêm video (<nw class="count">0</nw>/1)')
        product_detail.ShowProductTab()
        $('.add-product').removeClass('placeholder')
        $('.add-product').removeClass('box-placeholder')
        $('#single-product-amount').show()
        $('#product-attributes-price').hide()
        $('#product-attributes-all-price').closest('.item-edit').hide()
        $('.select2').each(function (index, item) {
            var element = $(this)
            element.select2({
                minimumResultsForSearch: Infinity
            });
        })
        $('#group-product-selection').html('')
        $('#group-id input').attr('placeholder', 'Chọn ngành hàng')
        $('#group-id input').attr('data-id', '-1')
        _product_function.POST('/Product/GroupProduct', { group_id: 1 }, function (result) {
            if (result.is_success && result.data) {
                $('#them-nganhhang .bg-box .row').html('')
                var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                var html_item = ''
                $(result.data).each(function (index, item) {
                    html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                        .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                })
                html = html.replace('{li}', html_item).replaceAll('{name}', 'HuloToy').replaceAll('{level}', '0')
                $('#them-nganhhang .bg-box .row').html(html)
            }
        });
        $('#specifications .datepicker-input').each(function (index, item) {
            var element = $(this)
            SingleDatePicker(element, 'down')
        })
    },
    RenderExistsProduct: function (product) {
        //--Init
        $('.add-product').removeClass('placeholder')
        $('.add-product').removeClass('box-placeholder')
        $('#images .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', _product_constants.VALUES.ProductDetail_Max_Image))
        $('#avatar .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list span').html('Thêm video (<nw class="count">0</nw>/1)')
        product_detail.ShowProductTab()

        //-- Image
        $(product.images).each(function (index, item) {
            if (item == null || item.trim() == '') return true
            var img_src = item
            if (!img_src.includes(product_detail_constants.StaticDomain)
                && !img_src.includes("data:image")
                && !img_src.includes("http"))
                img_src = _product_constants.VALUES.StaticDomain + item
            if (!(item.includes('data:image') && item.includes('base64')) || !(item.startsWith("http"))) {
                img_src = _product_constants.VALUES.StaticDomain+item
            }
            $('#images .list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', img_src).replaceAll('{id}', '-1'))
            $('#images .items .count').html($('#images .items .count').closest('.list').find('.magnific_popup').length)

        })
        //-- Avatar
        var img_src = product.avatar
        if (!img_src.includes(product_detail_constants.StaticDomain)
            && !img_src.includes("data:image")
            && !img_src.includes("http"))
            img_src = _product_constants.VALUES.StaticDomain + product.avatar
       
        $('#avatar .list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', img_src).replaceAll('{id}', '-1'))
        $('#avatar .items .count').html($('#avatar .items .count').closest('.list').find('.magnific_popup').length)

        $(product.videos).each(function (index, item) {
            $('#videos .list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', item).replaceAll('{id}', '-1'))
            $('#videos .items .count').html($('#videos .items .count').closest('.list').find('.magnific_popup').length)

        })
        
        $('#product-name input').val(product.name)

        //-- Group Product
        $('#group-id input').attr('data-id', product.group_product_id)
        _product_function.POST('/Product/GroupProduct', { group_id: 1 }, function (result) {
            if (result.is_success && result.data) {
                $('#them-nganhhang .bg-box .row').html('')
                var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                var html_item = ''
                $(result.data).each(function (index, item) {
                    html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                        .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                })
                html = html.replace('{li}', html_item).replaceAll('{name}', 'HuloToy').replaceAll('{level}', '0')
                $('#them-nganhhang .bg-box .row').html(html)
                //$('.select-group-product').trigger('click')
                $('#them-nganhhang .col-md-4').first().find('li[data-id="' + product.group_product_id.split(',')[0] + '"]').trigger('click')
                var group_split = product.group_product_id.split(',')

                for (var i = 1; i < group_split.length; i++) {
                    setTimeout(function () {
                        $('#them-nganhhang li[data-id="' + group_split[i] + '"]').trigger('click')
                        product_detail.RenderSelectedGroupProduct()
                    }, 100);
                }
            }
        });
        
        //-- Specification
        var html = ''

        $(_product_constants.VALUES.DefaultSpecificationValue).each(function (index, item) {
            var specification = product.specification.filter(obj => {
                return obj.attribute_id === item.attribute_id
            })
            switch (item.type) {
                case 1: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_Input
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', specification.length>0? specification[0].value:'')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type)
                        .replaceAll('{name}', item.name)
                        .replaceAll('{wrap_input}', html_item)


                } break;
                case 2: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_DateTime
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', specification.length > 0 ? specification[0].value : '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)


                } break;
                default: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', specification.length > 0 ? specification[0].value : '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)




                } break;
            }
        })

        $('.specifications-box').html(html)
        $('#specifications .datepicker-input').each(function (index, item) {
            var element = $(this)
            SingleDatePicker(element, 'down')
        })
        $('#description textarea').val(product.description)
        $('#main-price input').val(product.price)
        $('#main-amount input').val(product.amount)
        $('#main-stock input').val(product.stock)
        $('#main-sku input').val(product.sku)
        $('#main-profit input').val(product.profit)
        if (product.attributes != undefined && product.attributes.length > 0) {
            $('#single-product-amount').hide()
            $('#product-attributes-box').show()
        } else {
            $('#single-product-amount').show()
            $('#product-attributes-box').hide()
        }
        //Attribute:
        $('#product-attributes-box').html('')
        for (var i = 0; i < product.attributes.length; i++) {
            product_detail.AddProductAttributes()
        }
        $('.product-attributes').each(function (index, item) {
            var element = $(this)
            var attribute = product.attributes.filter(obj => {
                return obj._id == ('' + (index + 1))
            })
            element.find('.attribute-name').val(attribute[0].name)
            element.find('.edit-attributes-name-confirm').trigger('click')
            //element.find('.row-attributes-value').html('')
            var attribute_detail = product.attributes_detail.filter(obj => {
                return obj.attribute_id == ('' + (index + 1))
            })
            var first=true
            $(attribute_detail).each(function (index_detail, item_detail) {
                $(element.find('.row-attributes-value').find('.col-md-6').find('.attributes-name')).each(function (index_element, item_element) {
                    if (index_element == index_detail) {
                        var element_attr_detail = $(this)
                        element_attr_detail.val(item_detail.name)
                        element_attr_detail.trigger('keyup')
                    }
                })
            })
        })
        product_detail.RenderRowAttributeTablePrice()
        _product_function.POST('/Product/ProductSubListing', { parent_id: product._id }, function (result) {
            if (result.is_success && result.data) {
                $('#product-attributes-price tbody tr').each(function (index, item) {
                    var element = $(this)
                    var list = result.data
                    for (var i = 1; i <= product.attributes.length; i++) {
                        list = list.filter(obj => {
                            //return obj.variation_attributes.includes({ level: i, name: element.attr('data-attribute-'+i)})
                            return obj.variation_detail.some(e => e.id == i && e.name == element.attr('data-attribute-' + i))
                        })
                    }
                    element.attr('data-id', list[0]._id)
                    element.find('.td-price').find('input').val(Comma(list[0].price))
                    element.find('.td-profit').find('input').val(Comma(list[0].profit))
                    element.find('.td-amount').find('input').val(Comma(list[0].amount))
                    element.find('.td-stock').find('input').val(Comma(list[0].quanity_of_stock))
                    element.find('.td-sku').find('input').val(list[0].sku)
                })

            }
        });
       

        if (product.discount_group_buy != undefined && product.discount_group_buy.length > 0) {
            $('.btn-add-discount-groupbuy').closest('.col-md-6').hide()
            $('#discount-groupbuy').show()
            for (var i = 1; i <= product.discount_group_buy.length; i++) {
                $('#discount-groupbuy .btn-add').trigger('click')
            }
            $('#discount-groupbuy tbody .discount-groupbuy-row').each(function (index, item) {
               var element = $(this)
                var selected = product.discount_group_buy[index]
                element.find('.quanity-from').find('input').val(Comma(selected.from))
                element.find('.quanity-to').find('input').val(Comma(selected.to))
                element.find('input[name="discount-type-' + (index+1) + '"][value=' + (selected.type <= 0 ? 0 : selected.type) + ']').prop('checked', 'checked')
                switch (selected.type) {
                    case 1: {
                        element.find('.discount-percent').find('input').val(Comma(selected.discount))

                    } break;
                    default: {
                        element.find('.discount-number').find('input').val(Comma(selected.discount))
                    } break;
                    
                }
            })
        } else {
            $('.btn-add-discount-groupbuy').closest('.col-md-6').show()
            $('#discount-groupbuy').hide()
        }
        $('#other-information input[name="preorder_status"][value=' + product.preorder_status + ']').prop('checked', 'checked')
        $('#condition_of_product').val(product.condition_of_product).trigger('change')
        $('#sku input').val(product.sku)
        
    },
    RenderSpecificationLi: function (element) {
        var html = ''
        var type = element.closest('.col-md-6').find('.namesp').attr('data-attr-id')
        var name = element.closest('.col-md-6').find('.input_search').val()
        
        _product_function.POST('/Product/GetSpecificationByName', { type: type, name: name }, function (result) {
            if (result.is_success && result.data && result.data.length > 0) {
                var current_value = element.closest('.namesp').find('.input-select-option').val()
                if (current_value == undefined) current_value=''
                $(result.data).each(function (index, item) {
                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions_NewOptions
                        .replaceAll('{option-name}', 'specification-' + type)
                        .replaceAll('{value}', item._id)
                        .replaceAll('{checked}', current_value.includes(item.attribute_name)?'checked':'' )
                        .replaceAll('{name}', item.attribute_name)
                })
            }
            element.closest('.col-md-6').find('ul').html(html)
            
        });
    },
    RenderSpecificationSelectOption: function (element) {
        var value = ''
        var html=''
        element.closest('ul').find('input:checked').each(function (index, item) {
            var checkbox_element = $(this)
            value += checkbox_element.val()
            html += checkbox_element.closest('li').find('span').html() 
            if (index < (element.closest('ul').find('input:checked').length-1)) {
                value += ','
                html += ','
            }
        });
        element.closest('.col-md-6').find('.namesp').find('.input-select-option').attr('data-value', value)
        element.closest('.col-md-6').find('.namesp').find('.input-select-option').val(html)
    },
    RenderOnSelectGroupProduct: function (element_selected) {
        var html_selected_popup = ''
        var lastest_group_id = 0
        var level = 0
        var lastest_group_name = ''
        var selected_md4_level = parseInt(element_selected.closest('.col-md-4').attr('data-level'))
        $('#them-nganhhang .col-md-4').each(function (index, item) {
            var element = $(this)
            if (index > parseInt(selected_md4_level)) {
                element.remove()
            }
        })
        $('#them-nganhhang .col-md-4').each(function (index, item) {
            var element = $(this)
            var selected = element.find('ul').find('.active').attr('data-name')
            if (index >= ($('#them-nganhhang .col-md-4').length - 1)) {
                html_selected_popup += _product_constants.HTML.ProductDetail_GroupProduct_ResultSelected.replaceAll('{name}', element.find('ul').find('.active').attr('data-name'))
            } else {
                
                html_selected_popup += _product_constants.HTML.ProductDetail_GroupProduct_ResultDirection.replaceAll('{name}', selected)
            }
           
            lastest_group_id = element.find('.active').attr('data-id')
            level = index
            lastest_group_name = element.find('ul').find('.active').attr('data-name')
        })
        $('#group-product-selection').html(html_selected_popup)

        _product_function.POST('/Product/GroupProduct', { group_id: parseInt(lastest_group_id) }, function (result) {
            if (result.is_success && result.data && result.data.length>0) {
                var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                var html_item = ''
                $(result.data).each(function (index, item) {
                    html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                        .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                })
                html = html.replace('{li}', html_item).replaceAll('{name}', lastest_group_name).replaceAll('{level}', (level+1))
                $('#them-nganhhang .bg-box .row').append(html)

            }
        });
    },
    RenderSelectedGroupProduct: function () {
        var html_selected_input = ''
        var group_selected =''
        $('#them-nganhhang .col-md-4').each(function (index, item) {
            var element = $(this)
            var selected = element.find('ul').find('.active').attr('data-name')
            if (element.find('ul').find('.active').attr('data-id') == undefined) return true
            if (index >= ($('#them-nganhhang .col-md-4').length - 1)) {
                html_selected_input += selected
                group_selected += element.find('ul').find('.active').attr('data-id')
            } else {

                html_selected_input += selected + ' > '
                group_selected += element.find('ul').find('.active').attr('data-id')+','

            }
        })
        $('#group-id input').val(html_selected_input)
        $('#group-id input').attr('data-id', group_selected )

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
        var max_item = _product_constants.VALUES.ProductDetail_Max_Image
        switch (element.closest('.flex-lg-nowrap').attr('id')) {
            case 'images': {

            } break
            case 'avatar': {
                max_item = _product_constants.VALUES.ProductDetail_Max_Avt
            } break
            case 'videos': {
                max_item = _product_constants.VALUES.ProductDetail_Max_Avt

            } break
        }
        if (element.closest('.flex-lg-nowrap').find('.magnific_popup').length >= max_item) {
            _msgalert.error('Số lượng ảnh/videos vượt quá giới hạn')


            element.val(null)
        }
        else {
            $(element[0].files).each(function (index, item) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    element.closest('.list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', e.target.result).replaceAll('{id}', '-1'))
                    element.closest('.items').find('.count').html(element.closest('.list').find('.magnific_popup').length)

                }
                reader.readAsDataURL(item);

            });
            element.val(null)

        }

    },
    Summit: function () {
        var model = {
            _id: $('#product_detail').val() == undefined || $('#product_detail').val().trim() == '' ? null : $('#product_detail').val(),
            code: $('#product_detail').attr('data-code') == undefined || $('#product_detail').attr('data-code').trim() == '' ? null : $('#product_detail').attr('data-code'),
            amount: $('#main-amount input').val() == undefined || $('#main-amount input').val().trim() == ''?0: parseFloat($('#main-amount input').val().replaceAll(',','')),
            discount: 0,
            quanity_of_stock: $('#main-stock input').val() == undefined || $('#main-stock input').val().trim() == '' ? 0 : parseInt($('#main-stock input').val().replaceAll(',', ''))
        }
        model.images =[]
        $('#images .list .items').each(function (index, item) {
            var element_image = $(this)
            if (element_image.find('img').length > 0) {
                model.images.push(element_image.find('img').attr('src'))

            }
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
        $('#specifications .namesp').each(function (index, item) {
            var element = $(this)

            model.specification.push({
                _id: '-1',
                attribute_id: element.attr('data-attr-id'),
                value_type: element.attr('data-type'),
                value: element.find('input').val()
            })

        })
        var attribute_model =  product_detail.GetAttributeItem()
        model.attributes = attribute_model.attributes
        model.attributes_detail = attribute_model.attributes_detail
        
        
        model.discount_group_buy = []
        $('#discount-groupbuy tbody .discount-groupbuy-row').each(function (index, item) {
            var element = $(this)
            var from = element.find('.quanity-from').find('input').val() == undefined || element.find('.quanity-from').find('input').val().trim() == '' ? 0 : parseInt(element.find('.quanity-from').find('input').val().replaceAll(',',''))
            var to = element.find('.quanity-to').find('input').val() == undefined || element.find('.quanity-to').find('input').val().trim() == '' ? 0 : parseInt(element.find('.quanity-to').find('input').val().replaceAll(',',''))
            var to = element.find('.quanity-to').find('input').val() == undefined || element.find('.quanity-to').find('input').val().trim() == '' ? 0 : parseInt(element.find('.quanity-to').find('input').val().replaceAll(',', ''))
            var checkbox_value = element.find('input[name="discount-type-' + (index + 1) +'"]:checked').val()

            var discount=0
            switch (checkbox_value) {
                case '0': {
                    discount = parseFloat(element.find('.discount-number').find('input').val().replaceAll(',',''))
                } break
                case '1': {
                    discount = parseFloat(element.find('.discount-percent').find('input').val().replaceAll(',', ''))

                } break
            }
            model.discount_group_buy.push({
                from: from,
                to: to,
                discount: discount,
                type: parseInt(checkbox_value)
            })
        })
        
        model.variations = []
        $('#product-attributes-price tbody tr').each(function (index, index) {
            var element = $(this)
            var var_id = element.attr('data-id')
            if (var_id = undefined) var_id =''
            var variation = {
                _id: var_id,
                variation_attributes: [],
                price: parseFloat(element.find('.td-price').find('input').val().replaceAll(',', '')),
                profit: parseFloat(element.find('.td-profit').find('input').val().replaceAll(',', '')),
                amount: parseFloat(element.find('.td-amount').find('input').val().replaceAll(',', '')),
                quanity_of_stock: parseFloat(element.find('.td-stock').find('input').val().replaceAll(',', '')),
                sku: element.find('.td-sku').find('input').val(),
            }
            for (var i = 1; i <= $('.product-attributes').length; i++) {
                var attr_value = element.attr('data-attribute-' + i)
                
                variation.variation_attributes.push({
                    id: i,
                    name: attr_value
                })
            }
            model.variations.push(variation)
        })
        
        model.preorder_status = $('input[name="preorder_status"]') == 1 ? true : false
        model.condition_of_product = $('#condition_of_product').find(':selected').val()
        model.sku = $('#sku input').val()
        
        _product_function.POST('/Product/Summit', { request: model }, function (result) {
            if (result.is_success) {
                _msgalert.success('Thêm mới sản phẩm thành công')
                setTimeout(function () {
                    window.location.href = '/product/detail';
                }, 2000);
            }
            else {
                _msgalert.error(result.msg)

            }
        });
    },
    GetLevelOfAttributesBox: function (element) {
       var result=0
        $('.product-attributes').each(function (index, item) {
            if ($(this).is(element)) {
                result = (index + 1)
                return false
            }
        })
        return result
    },
    Validate: function () {
        var validated = true


        return validated
    },
    AddProductAttributes: function () {
        var attribute_max_count = 2
        if ($('.product-attributes').length < attribute_max_count) {
            $('#product-attributes-box').append(_product_constants.HTML.ProductDetail_Attribute_Row.replaceAll('{html}', _product_constants.HTML.ProductDetail_Attribute_Row_Item))
            //if ($('.product-attributes').length < attribute_max_count) {
            //    $('#product-attributes-box').append(_product_constants.HTML.ProductDetail_Attribute_Row_Add_Attributes)
            //}
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
    },
    RenderRowAttributeTablePrice: function () {
        var html=''
        var product_attributes = []
        $('.product-attributes').each(function (index, item) {
            var element = $(this)
            var product_attribute_by_id = {
                id: index,
                data_values:[]
            }
            element.find('.attributes-name').each(function (index, item) {
                var element_input = $(this)
                if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '')
                product_attribute_by_id.data_values.push(element_input.val())
            })
            product_attributes.push(product_attribute_by_id)
        })
        var combination_array = []
        if (product_attributes.length > 0) {
            combination_array = product_attributes[0].data_values.map(v => [].concat(v))
            if (product_attributes.length > 1) {
                for (var i = 1; i < product_attributes.length; i++) {
                    var array2 = product_attributes[i].data_values;
                    combination_array = combination_array.flatMap(d => array2.map(v => [].concat(d, v)))

                }
            }
        }
       
        if (combination_array.length > 0) {
            var name = combination_array[0][0]
            var model = product_detail.GetAttributeItem()
            $(combination_array).each(function (index, item) {
                var html_attribute_attr=''
                var html_td_attribute = ''

                $(item).each(function (index_attribute, attribute_name) {         
                    var row_span = model.attributes_detail.filter(obj => {
                        return obj.attribute_id == (index_attribute + 2)
                    }).length
                    if (row_span <= 0) row_span = 1

                    var html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                        .replaceAll('{i}', index_attribute)
                        .replaceAll('{name}', attribute_name.trim())
                        .replaceAll('{row_span}', 'rowspan="' + row_span + '"')
                    html_attribute_attr += 'data-attribute-' + (index_attribute + 1) + '="' + attribute_name.trim() + '" '
                    html_td_attribute += html_item;
                    
                })

                
                

                if (item[0].toLowerCase().trim() == name) {
                    html += _product_constants.HTML.ProductDetail_Attribute_Price_TrSub
                        .replaceAll('data-attribute-1="Phân loại 1" data-attribute-2="Phân loại 2-2"', html_attribute_attr)
                        .replaceAll('{td_arrtibute}', html_td_attribute)
                }
                else {
                    name = item[0].toLowerCase().trim()
                    html += _product_constants.HTML.ProductDetail_Attribute_Price_TrMain
                        .replaceAll('data-attribute-1="Phân loại 1" data-attribute-2="Phân loại 2-1"', html_attribute_attr)
                        .replaceAll('{td_arrtibute}', html_td_attribute)
                }

            })

        }
        $('#product-attributes-price tbody').html(html)
        $('#product-attributes-price .th-attributes').hide()
        for (var i = 1; i <= $('.product-attributes').length; i++) {
            $('#product-attributes-price .th-attribute-' + i).show()
            $('#product-attributes-price .th-attribute-' + i).html($($('.product-attributes')[(i-1)]).find('h6').find('b').text())

        }
        var list_filter = GetAttributeList(1)
        $(list_filter).each(function (index, item) {
            HideAttributes(item,1)
        })
    },

    ApplyAllPriceToTable: function () {
        $('#product-attributes-price .td-price input').val(Comma($('#product-attributes-all-price .td-price input').val()))
        $('#product-attributes-price .td-profit input').val(Comma($('#product-attributes-all-price .td-profit input').val()))
        $('#product-attributes-price .td-stock input').val(Comma($('#product-attributes-all-price .td-stock input').val()))
        $('#product-attributes-price .td-sku input').val($('#product-attributes-all-price .td-sku input').val())
        var price = isNaN(parseFloat($('#product-attributes-all-price .td-price input').val().replaceAll(',', ''))) ? 0 : parseFloat($('#product-attributes-all-price .td-price input').val().replaceAll(',', ''))
        var profit = isNaN(parseFloat($('#product-attributes-all-price .td-profit input').val().replaceAll(',', ''))) ? 0 : parseFloat($('#product-attributes-all-price .td-profit input').val().replaceAll(',', ''))
        $('#product-attributes-price .td-amount input').val(Comma(price + profit))
        //$('#product-attributes-all-price .td-price input').val('')
        //$('#product-attributes-all-price .td-profit input').val('')
        //$('#product-attributes-all-price .td-stock input').val('')
        //$('#product-attributes-all-price .td-sku input').val('')
    },
    UpdateRowData: function (tr) {
        if (tr.find('.td-price').length > 0 && tr.find('.td-profit').length > 0 && tr.find('.td-amount').length > 0) {
            var price = isNaN(parseFloat(tr.find('.td-price').find('input').val().replaceAll(',', ''))) ? 0 : parseFloat(tr.find('.td-price').find('input').val().replaceAll(',', ''))
            var profit = isNaN(parseFloat(tr.find('.td-profit').find('input').val().replaceAll(',', ''))) ? 0 : parseFloat(tr.find('.td-profit').find('input').val().replaceAll(',', ''))

            tr.find('.td-amount').find('input').val(Comma(price + profit))
        }
        
    },
    GetAttributeItem: function () {
        var model = {

        }
        model.attributes = []
        $('.product-attributes').each(function (index, item) {
            var element = $(this)
            model.attributes.push({
                _id: (index + 1),
                name: element.find('h6').find('b').html(),
            })

        })
        model.attributes_detail = []
        $('#product-attributes-box .attributes-name').each(function (index_2, item_2) {
            var element = $(this)
            var value = element.val()
            if (value != undefined && value.trim() != '') {
                model.attributes_detail.push({
                    attribute_id: product_detail.GetLevelOfAttributesBox(element.closest('.product-attributes')),
                    img: '',
                    name: element.val()
                })
            }
        })
        return model
    }
    
}
function SingleDatePicker(element, dropdown_position = 'down') {
    var today = new Date();
    var yyyy = today.getFullYear();
    var mm = today.getMonth() + 1; // Months start at 0!
    var dd = today.getDate();
    var yyyy_max = yyyy + 10;
    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;
    var time_now = dd + '/' + mm + '/' + yyyy ;
    var max_range = '31/12/' + yyyy_max;

    element.daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        showDropdowns: true,
        drops: dropdown_position,
        minDate: time_now,
        maxDate: max_range,
        locale: {
            format: 'DD/MM/YYYY'
        }
    }, function (start, end, label) {


    });
    if (element.val() != undefined && element.val().trim() != '') {
        element.data('daterangepicker').setStartDate();
    }

}
function HideAttributes(name, i) {
    var find = $('#product-attributes-price tbody tr[data-attribute-' + i + '="' + name + '"]')
    if (find.length <= 0) return;
    var first = true;
    
    find.each(function (index, item) {
        var element=$(this)
        if (first) {
            first = false
        } else {
            for (var field_index = 0; field_index < i; field_index++) {
                $(element.find('.td-attribute-' + field_index)).hide()
            }
        }

    })
    //var list = GetAttributeList(++i)
    //$(list).each(function (index, item) {
    //    HideAttributes(item,i)
    //})
}
function GetAttributeList(index) {
    var list = []
    $($('#product-attributes-box .row-attributes-value')[index-1]).find('input[type="text"]').each(function (index_item, item) {
        var element = $(this)
        list.push(element.val())

    })
    return list
}
function Comma(number) { //function to add commas to textboxes
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
}