import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnFundtransferapprovalComponent } from './acc-trn-fundtransferapproval.component';

describe('AccTrnFundtransferapprovalComponent', () => {
  let component: AccTrnFundtransferapprovalComponent;
  let fixture: ComponentFixture<AccTrnFundtransferapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnFundtransferapprovalComponent]
    });
    fixture = TestBed.createComponent(AccTrnFundtransferapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
