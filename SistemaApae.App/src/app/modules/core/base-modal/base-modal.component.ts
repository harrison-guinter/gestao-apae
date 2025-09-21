import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

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
  @Input() modalTitle!: string;
  @Input() confirmButtonText: string = 'Confirmar';
  @Input() formInvalid: boolean = false;

  @Output() onConfirmClick = new EventEmitter<void>();
  @Output() onCancelClick = new EventEmitter<void>();

  constructor(public dialogRef: MatDialogRef<BaseModalComponent>) {}

  ngOnInit(): void {}

  onCancel(): void {
    this.onCancelClick.emit();
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.onConfirmClick.emit();
    this.dialogRef.close(true);
  }
}
