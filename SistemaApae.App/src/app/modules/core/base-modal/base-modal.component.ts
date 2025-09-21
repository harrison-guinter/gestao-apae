import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ModalData } from '../services/modal.service';

@Component({
  selector: 'app-base-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
  ],
  templateUrl: './base-modal.component.html',
  styleUrls: ['./base-modal.component.less'],
})
export class BaseModalComponent implements OnInit {
  confirmButtonText: string = 'Confirmar';

  constructor(
    public dialogRef: MatDialogRef<BaseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ModalData
  ) {}

  ngOnInit(): void {
    this.initFormCadastro();
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onConfirm(): void {
    if (this.data.element?.valid) {
      this.dialogRef.close(this.data.element.value);
    } else {
      this.dialogRef.close(this.data.element);
    }
  }

  isFormInvalid(): boolean {
    return this.data.element && this.data.element.invalid ? this.data.element.invalid : false;
  }

  getElement(): any {
    return this.data.element || null;
  }

  protected initFormCadastro(): void {}
}
