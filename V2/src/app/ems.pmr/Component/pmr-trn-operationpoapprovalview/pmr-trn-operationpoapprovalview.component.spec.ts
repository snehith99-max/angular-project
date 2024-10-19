import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnOperationpoapprovalviewComponent } from './pmr-trn-operationpoapprovalview.component';

describe('PmrTrnOperationpoapprovalviewComponent', () => {
  let component: PmrTrnOperationpoapprovalviewComponent;
  let fixture: ComponentFixture<PmrTrnOperationpoapprovalviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnOperationpoapprovalviewComponent]
    });
    fixture = TestBed.createComponent(PmrTrnOperationpoapprovalviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
