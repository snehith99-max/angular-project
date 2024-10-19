import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayTrnPayruneditComponent } from './pay-trn-payrunedit.component';

describe('PayTrnPayruneditComponent', () => {
  let component: PayTrnPayruneditComponent;
  let fixture: ComponentFixture<PayTrnPayruneditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayTrnPayruneditComponent]
    });
    fixture = TestBed.createComponent(PayTrnPayruneditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
