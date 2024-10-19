import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTcreditnoteviewComponent } from './smr-trn-tcreditnoteview.component';

describe('SmrTrnTcreditnoteviewComponent', () => {
  let component: SmrTrnTcreditnoteviewComponent;
  let fixture: ComponentFixture<SmrTrnTcreditnoteviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTcreditnoteviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTcreditnoteviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
