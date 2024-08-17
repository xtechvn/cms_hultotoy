$(document).ready(function () {
    product_index.Initialization()
})
var product_index = {
    Model: {
        keyword: '',
        group_id: -1,
        page_index: 1,
        page_size: 10
    },
    Initialization: function () {
        var model = [{ url: '/', name: 'Trang chủ' }, { url: '/productv2', name: 'Quản lý sản phẩm', activated: true }]
        _global_function.RenderBreadcumb(model)
        product_index.Listing();
        product_index.DynamicBind()
    },
    DynamicBind: function () {
        $('body').on('click', '.btn-add-product', function () {
            window.location.href='/product/detail'
        });
       
        $('body').on('click', '.product-edit, .name-product', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            if (product_id != null && product_id != undefined && product_id.trim() != '') {
                window.location.href = '/product/detail/' + product_id

            }
        });
        $('body').on('click', '.product-viewmore', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            if (product_id != null && product_id != undefined && product_id.trim() != '') {
                window.location.href = '/product/detail/' + product_id

            }
        });
        $('body').on('click', '.btn-export-excel', function () {

        });
    },
    Listing: function () {
        _product_function.POST('/Product/ProductListing', product_index.Model, function (result) {
            if (result.is_success && result.data && result.data.length > 0) {
                product_index.RenderSearch(result.data)
            }
            else {
                $('#product_list').html('')
            }
            
        });

    },
    RenderSearch: function (main_products) {
        var html = ''
        $(main_products).each(function (index, item) {
            
            var html_item = _product_constants.HTML.Product
            html_item = html_item.replaceAll('{id}', item._id)
            html_item = html_item.replaceAll('{avatar}', item.avatar)
            html_item = html_item.replaceAll('{name}', item.name)           
            html_item = html_item.replaceAll('{attribute}', '')
            if (item.amount > 0) {
                html_item = html_item.replaceAll('{amount}', _product_function.Comma(item.amount) +' đ')
            } 
            if (item.quanity_of_stock > 0) {
                html_item = html_item.replaceAll('{stock}', _product_function.Comma(item.quanity_of_stock))
            } 
            html_item = html_item.replaceAll('{order_count}', '')
            var html_variations=''
            if (item.variations && item.variations.length > 0) {
                var amount=[]
                var quanity_stock=[]
                $(item.variations).each(function (index, sub_item) {
                    var html_sub_item = _product_constants.HTML.SubProduct
                        .replaceAll('{id}', item._id)
                        .replaceAll('{main_id}', item._id)
                        .replaceAll('{name}', sub_item.name)
                        .replaceAll('{sku}', sub_item.sku)
                        .replaceAll('{amount}', _product_function.Comma(sub_item.amount) + ' đ')
                        .replaceAll('{stock}', _product_function.Comma(sub_item.quanity_of_stock))
                        .replaceAll('{order_count}', '')
                    var html_sub_attr = ''

                    //var result = jsObjects.filter(obj => {
                    //    return obj.b === 6
                    //})
                    var sub_attr_img=[]
                    $(sub_item.variation_attributes).each(function (index_variation_attributes, variation_attributes_item) {
                        var attribute = item.attributes.filter(obj => {
                            return obj._id == variation_attributes_item.level
                        })
                        var attribute_detail = item.attributes_detail.filter(obj => {
                            return (obj.attribute_id == variation_attributes_item.level && obj.name == variation_attributes_item.name)
                        })
                        if (attribute[0].img != null && attribute[0].img != undefined && attribute[0].img.trim() != '') {
                            sub_attr_img.push(attribute[0].img)
                        }
                        if (attribute_detail[0].img != null && attribute_detail[0].img != undefined && attribute_detail[0].img.trim() != '') {
                            sub_attr_img.push(attribute_detail[0].img)
                        }
                        html_sub_attr += '' + attribute[0].name + ': ' + attribute_detail[0].name
                        if (index_variation_attributes < ($(sub_item.variation_attributes).length - 1)) {
                            html_sub_attr += ', '
                        }
                        
                    })
                    html_sub_item = html_sub_item.replaceAll('{attribute}','Phân loại hàng: '+ html_sub_attr)
                    html_sub_item = html_sub_item.replaceAll('{avatar}', sub_attr_img.length > 0 ? sub_attr_img[0] : item.avatar)
                    html_variations += html_sub_item
                    amount.push(sub_item.amount)
                    quanity_stock.push(sub_item.quanity_of_stock)
                });
                const sum_stock = quanity_stock.reduce((partialSum, a) => partialSum + a, 0);
                var max = Math.max(...amount);
                var min = Math.min(...amount);
                html_item = html_item.replaceAll('{amount}', _product_function.Comma(min) + ' đ - ' + _product_function.Comma(max) + ' đ')
                html_item = html_item.replaceAll('{stock}', _product_function.Comma(sum_stock))

                
            }
            html += html_item
            html += html_variations

        });
        $('#product_list').html(html)
        
    }
   
}
