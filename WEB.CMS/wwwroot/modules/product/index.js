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
        $('body').on('click', '.product-edit', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            if (product_id != null && product_id != undefined && product_id.trim() != '') {
                window.location.href = '/product/detail/' + product_id

            }
        });
        $('body').on('click', '.product-viewmore', function () {

        });
        $('body').on('click', '.btn-export-excel', function () {

        });
    },
    Listing: function () {
        _product_function.POST('/Product/ProductListing', product_index.Model, function (result) {
            if (result.is_success && result.data && result.data.length > 0) {
                var model_sub = {
                    main_products: result.data.map(a => a._id)
                }
                _product_function.POST('/Product/ProductSubListing', model_sub, function (result_sublisting) {
                    product_index.RenderSearch(result.data, result_sublisting.data)

                })
            }
            else {
                $('#product_list').html('')
            }
            
        });

    },
    RenderSearch: function (main_products, sub_products) {
        var html = ''
        $(main_products).each(function (index, item) {
            var html_item = _product_constants.HTML.Product
                .replaceAll('{id}', item._id)
                .replaceAll('{avatar}', item.avatar)
                .replaceAll('{name}', item.name)
            var html_attr = ''
            $(item.attributes).each(function (index_attr, attr) {
                html_attr += attr.value + ', '
            });
            html_item.replaceAll('{attribute}', html_attr)
            html_item.replaceAll('{amount}', _product_function.Comma(item.amount))
            html_item.replaceAll('{stock}', _product_function.Comma(item.quanity_of_stock))
            var sub_by_id = sub_products.filter(obj => {
                return obj.parent_product_id == item._id
            })
            html += html_item
            if (sub_by_id && sub_by_id.length > 0) {
                $(main_products).each(function (index, sub_item) {
                    var html_sub_item = _product_constants.HTML.SubProduct
                        .replaceAll('{id}', sub_item._id)
                        .replaceAll('{main_id}', sub_item.parent_product_id)
                        .replaceAll('{avatar}', sub_item.avatar)
                        .replaceAll('{name}', sub_item.name)
                        .replaceAll('{sku}', sub_item.sku)
                        .replaceAll('{amount}', _product_function.Comma(sub_item.amount))
                        .replaceAll('{stock}', _product_function.Comma(sub_item.quanity_of_stock))
                    var html_sub_attr = ''
                    $(item.attributes).each(function (index_attr, sub_attr) {
                        html_sub_attr += sub_attr.value + ', '
                    });
                    html_sub_item.replaceAll('{attribute}', html_sub_attr)
                    html += html_sub_item

                });
            }
        });
        $('#product_list').html(html)
    }
   
}
