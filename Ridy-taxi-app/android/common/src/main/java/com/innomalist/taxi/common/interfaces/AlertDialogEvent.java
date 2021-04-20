package com.innomalist.taxi.common.interfaces;

import com.innomalist.taxi.common.utils.AlertDialogBuilder;

public interface AlertDialogEvent {
    void onAnswerDialog(AlertDialogBuilder.DialogResult result);
}