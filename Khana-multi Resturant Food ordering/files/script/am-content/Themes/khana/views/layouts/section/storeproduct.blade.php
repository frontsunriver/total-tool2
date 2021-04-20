@foreach($categories as $category)
@if(count($category->products) > 0)
<div class="single-category-food mt-50">
	<div class="single-category-main-content">
		<div class="row">
			<div class="col-lg-12">
				<div class="category-title">
					<h3>{{ $category->name }}</h3>
				</div>
			</div>
		</div>
		<div class="row">
			@foreach($category->products as $product)
			<div class="col-lg-4 mb-30">
				<div class="single-food-card" @if($product->addons->count() > 0)  onclick="product_addon('{{ $product->slug }}','{{ $store->slug }}')" @else onclick="product_add_to_cart('{{ $product->slug }}','{{ $store->slug }}')" @endif>
					@if(!empty($product->preview->content))
					<div class="food-img">
						@php
						$thumbnail=ImageSize($product->preview->content,'medium');
						@endphp
						<img src="{{ asset($thumbnail) }}" alt="">
					</div>
					@endif
					<div class="food-another">
						<h5>{{ $product->title }}</h5>
						<p>{{ $product->excerpt->content ?? '' }}</p>
						<div class="food-price-action d-flex">
							@php
							$currency=\App\Options::where('key','currency_name')->select('value')->first();
							@endphp
							<span class="food-price">{{ strtoupper($currency->value) }} {{ number_format($product->price->price,2) }}</span>
							<div class="food-action">
								<a href="javascript:void(0)"><i class="fas fa-plus"></i></a>
							</div>
						</div>
					</div>
				</div>
			</div>
			@endforeach
		</div>
	</div>
</div>
@endif
@endforeach
