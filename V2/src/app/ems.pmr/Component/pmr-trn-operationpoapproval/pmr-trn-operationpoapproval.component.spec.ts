import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnOperationpoapprovalComponent } from './pmr-trn-operationpoapproval.component';

describe('PmrTrnOperationpoapprovalComponent', () => {
  let component: PmrTrnOperationpoapprovalComponent;
  let fixture: ComponentFixture<PmrTrnOperationpoapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnOperationpoapprovalComponent]
    });
    fixture = TestBed.createComponent(PmrTrnOperationpoapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
