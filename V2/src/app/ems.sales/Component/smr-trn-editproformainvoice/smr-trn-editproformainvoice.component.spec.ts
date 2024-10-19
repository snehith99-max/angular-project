import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnEditproformainvoiceComponent } from './smr-trn-editproformainvoice.component';

describe('SmrTrnEditproformainvoiceComponent', () => {
  let component: SmrTrnEditproformainvoiceComponent;
  let fixture: ComponentFixture<SmrTrnEditproformainvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnEditproformainvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnEditproformainvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
