import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskTrnMyApprovalComponent } from './tsk-trn-my-approval.component';

describe('TskTrnMyApprovalComponent', () => {
  let component: TskTrnMyApprovalComponent;
  let fixture: ComponentFixture<TskTrnMyApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskTrnMyApprovalComponent]
    });
    fixture = TestBed.createComponent(TskTrnMyApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
