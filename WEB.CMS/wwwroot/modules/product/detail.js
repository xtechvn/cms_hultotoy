$(document).ready(function () {
    product_detail.Initialization()
})
var product_detail = {
    Initialization: function () {
        var model = [{ url: '/', name: 'Trang chủ' }, { url: '/productv2', name: 'Quản lý sản phẩm' }, { url: 'javascript:;', name: 'Chi tiết sản phẩm',activated:true }]
        _global_function.RenderBreadcumb(model)
        $('.add-product').addClass('placeholder')
        $('.add-product').addClass('box-placeholder')

        product_detail.HideProductTab()
        product_detail.Detail()
        product_detail.DynamicBind()
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
        $('body').on('click', '.select-group-product', function () {
            var element = $(this)


        });
        $('body').on('keyup', '#description textarea, #product-name input', function () {
            var element = $(this)
            element.closest('.item').find('.count').html(_product_function.Comma(element.val().length))

        });
       
       
       
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
                    window.location.href='/error'
                }
            });
        }
        else {
            product_detail.RenderGeneralFunction()
            product_detail.RenderAddNewProduct()

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
     
    },
    RenderExistsProduct: function (product) {


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
}