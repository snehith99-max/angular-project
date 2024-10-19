import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesorderapprovalComponent } from './smr-trn-salesorderapproval.component';

describe('SmrTrnSalesorderapprovalComponent', () => {
  let component: SmrTrnSalesorderapprovalComponent;
  let fixture: ComponentFixture<SmrTrnSalesorderapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesorderapprovalComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesorderapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
