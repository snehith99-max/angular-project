import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnAddreceiptComponent } from './smr-trn-addreceipt.component';

describe('SmrTrnAddreceiptComponent', () => {
  let component: SmrTrnAddreceiptComponent;
  let fixture: ComponentFixture<SmrTrnAddreceiptComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnAddreceiptComponent]
    });
    fixture = TestBed.createComponent(SmrTrnAddreceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
