import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnCashbookentryaddComponent } from './acc-trn-cashbookentryadd.component';

describe('AccTrnCashbookentryaddComponent', () => {
  let component: AccTrnCashbookentryaddComponent;
  let fixture: ComponentFixture<AccTrnCashbookentryaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnCashbookentryaddComponent]
    });
    fixture = TestBed.createComponent(AccTrnCashbookentryaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
