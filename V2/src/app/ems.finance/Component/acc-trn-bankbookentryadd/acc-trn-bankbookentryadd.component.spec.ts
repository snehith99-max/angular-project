import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnBankbookentryaddComponent } from './acc-trn-bankbookentryadd.component';

describe('AccTrnBankbookentryaddComponent', () => {
  let component: AccTrnBankbookentryaddComponent;
  let fixture: ComponentFixture<AccTrnBankbookentryaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnBankbookentryaddComponent]
    });
    fixture = TestBed.createComponent(AccTrnBankbookentryaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
