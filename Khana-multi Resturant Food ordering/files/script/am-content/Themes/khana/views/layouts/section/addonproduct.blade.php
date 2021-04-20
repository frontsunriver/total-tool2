<div class="close-modal">
	<a class="close_model" onclick="close_model()" href="javascript:void(0)"><span class="ti-close"></span></a>
</div>
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
<div class="row">
	<div class="col-lg-6">
		<div class="food-title">
			<h4>{{ $product->title }}</h4>
		</div>
	</div>
	<div class="col-lg-6">
		<div class="right-section f-right">
			<span>{{ strtoupper($currency->value) }}: {{ number_format($product->price->price,2) }}</span>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-lg-12">
		<div class="food-description">
			<p>{{ $product->excerpt->content }}</p>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-lg-10">
		<div class="adds-on-area">
			<h5>Add-Ons on {{ $product->title }}</h5>
			<p>Select up to {{ $product->addons->count() }} (optional)</p>
		</div>
	</div>
	<div class="col-lg-2">
		<div class="adds-on-right-section">
			<span class="badges">
				{{ __('optional') }}
			</span>
		</div>
	</div>
</div>
<form action="{{ route('addonproduct.add_to_cart') }}" method="POST" id="addon_form">
	@csrf
	<div class="row">
		@foreach($product->addons as $addon)
		<div class="col-lg-12">
			<div class="row">
				<div class="col-lg-9">
					<div class="extra-product-addon d-flex">
						<div class="custom-control custom-checkbox">
							<input type="checkbox" name="addon[]" class="custom-control-input" id="{{ $addon->id }}" value="{{ $addon->id }}">
							<label class="custom-control-label" for="{{ $addon->id }}">{{ $addon->title }}</label>
						</div>
					</div>
				</div>
				<div class="col-lg-3">
					<div class="extra-product-price f-right">
						<span>{{ strtoupper($currency->value) }}: {{ number_format($addon->price->price,2) }}</span>
					</div>
				</div>
			</div>
		</div>
		@endforeach
		<input type="hidden" name="main_product" value="{{ $product->id }}">
		<input type="hidden" name="store_slug" value="{{ $store->slug }}">
	</div>
	<div class="row">
		<div class="col-lg-12">
			<div class="specials-area">
				<h5>{{ __('Special instructions') }}</h5>
				<p>{{ __('You can write down here any special instructions') }}</p>
			</div>
		</div>
		<div class="col-lg-12">
			<div class="modal-textarea">
				<textarea class="form-control" name="special_note" cols="30" rows="4"></textarea>
			</div>
		</div>
	</div>
	<div class="row mt-15">
		<div class="col-lg-2">
			<div class="modal-qty-section">
				<span class="ti-minus" onclick="addon_minus()"></span><span class="qty-value">1</span><span class="ti-plus" onclick="addon_plus()"></span>
				<input type="hidden" name="qty_value" id="qty_value_input" value="1">
			</div>
		</div>
		<div class="col-lg-10">
			<div class="add-to-cart-button">
				<button type="submit">{{ __('Add To Cart') }}</button>
			</div>
		</div>
	</div>
</form>

<script src="{{ theme_asset('khana/public/js/store/cart.js') }}"></script>