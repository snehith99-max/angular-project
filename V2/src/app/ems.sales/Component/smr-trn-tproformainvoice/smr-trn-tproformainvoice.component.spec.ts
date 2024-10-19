import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTproformainvoiceComponent } from './smr-trn-tproformainvoice.component';

describe('SmrTrnTproformainvoiceComponent', () => {
  let component: SmrTrnTproformainvoiceComponent;
  let fixture: ComponentFixture<SmrTrnTproformainvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTproformainvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTproformainvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
