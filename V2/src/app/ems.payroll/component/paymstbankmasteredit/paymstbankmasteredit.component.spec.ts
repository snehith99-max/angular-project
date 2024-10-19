import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymstbankmastereditComponent } from './paymstbankmasteredit.component';

describe('PaymstbankmastereditComponent', () => {
  let component: PaymstbankmastereditComponent;
  let fixture: ComponentFixture<PaymstbankmastereditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PaymstbankmastereditComponent]
    });
    fixture = TestBed.createComponent(PaymstbankmastereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
