package com.innomalist.taxi.common.utils

import android.widget.ImageView
import androidx.databinding.BindingAdapter
import com.bumptech.glide.Glide
import com.google.android.material.imageview.ShapeableImageView
import com.google.android.material.shape.ShapeAppearanceModel
import com.innomalist.taxi.common.Config
import com.innomalist.taxi.common.models.Media

object DataBinder {
    @JvmStatic
    @BindingAdapter("imgUrl")
    fun setImageUrl(imageView: ImageView, address: String?) {
        val context = imageView.context
        Glide.with(context).load(address).into(imageView)
    }

    @BindingAdapter("media")
    @JvmStatic
    fun setMedia(imageView: ImageView, media: Media?) {
        val context = imageView.context
        if (media == null) return
        val address = if (media.pathType === Media.PathType.absolute) media.address else "${Config.Backend}${media.address}"
        Glide.with(context).load(address).into(imageView)
    }

    @JvmStatic
    @BindingAdapter("media")
    fun setMedia(circleImageView: ShapeableImageView, media: Media?) {
        val context = circleImageView.context
        if (media == null) return
        val address = if (media.pathType === Media.PathType.absolute) media.address else "${Config.Backend}${media.address}"
        Glide.with(context).load(address).into(circleImageView)
    } /*@BindingAdapter("gender")
    public static void setGender(MaterialBetterSpinner spinner, Gender gender) {
        if(gender == null) {
            spinner.setText(spinner.getContext().getString(R.string.gender_unknown));
            return;
        }
        switch (gender) {
            case male:
                spinner.setText(spinner.getContext().getString(R.string.gender_male));
                break;

            case female:
                spinner.setText(spinner.getContext().getString(R.string.gender_female));
                break;

            default:

        }
    }
    @InverseBindingAdapter(attribute = "gender")
    public static Gender getGender(MaterialBetterSpinner spinner) {
        spinner.clearFocus();
        String unknown = spinner.getContext().getString(R.string.gender_unknown);
        String male = spinner.getContext().getString(R.string.gender_male);
        String female = spinner.getContext().getString(R.string.gender_female);
        if(spinner.getText().toString().equals(female))
            return Gender.female;
        if(spinner.getText().toString().equals(male))
            return Gender.male;
        return Gender.unknown;
    }

    @BindingAdapter(value = "genderAttrChanged")
    public static void bindGenderChanged(MaterialBetterSpinner pAppCompatSpinner, final InverseBindingListener newTextAttrChanged) {
        pAppCompatSpinner.setOnItemClickListener((adapterView, view, i, l) -> newTextAttrChanged.onChange());

    }*/
}