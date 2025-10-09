import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ConfirmationService } from '../confirmation-modal/confirmation.service';

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
  @Input() isVisualizacao?: boolean = false;
  @Input() formHasChanges: boolean = false;

  @Output() onConfirmClick = new EventEmitter<void>();
  @Output() onCancelClick = new EventEmitter<void>();

  @ViewChild('cancelButton') cancelButton!: MatButton;
  @ViewChild('closeButton') closeButton!: MatIcon;

  constructor(
    public dialogRef: MatDialogRef<BaseModalComponent>,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {}

  onClose(): void {
    const config = {
      message: this.confirmationService.msgAlteracaoNaoSalva,
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      elementRef: this.closeButton._elementRef.nativeElement,
      disableClose: true,
    };
    this.cancel(config);
  }

  onCancel(): void {
    const config = {
      message: this.confirmationService.msgAlteracaoNaoSalva,
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      elementRef: this.cancelButton._elementRef.nativeElement,
      disableClose: true,
    };
    this.cancel(config);
  }

  cancel(config: any) {
    if (this.formHasChanges) {
      this.confirmationService.openConfirmationModal(config).subscribe((confirmed) => {
        if (confirmed) {
          this.onCancelClick.emit();
        }
      });
    } else {
      this.onCancelClick.emit();
    }
  }

  onConfirm(): void {
    this.onConfirmClick.emit();
  }
}
