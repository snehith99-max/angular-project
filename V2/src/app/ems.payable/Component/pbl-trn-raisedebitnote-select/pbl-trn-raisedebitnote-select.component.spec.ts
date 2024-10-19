import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnRaisedebitnoteSelectComponent } from './pbl-trn-raisedebitnote-select.component';

describe('PblTrnRaisedebitnoteSelectComponent', () => {
  let component: PblTrnRaisedebitnoteSelectComponent;
  let fixture: ComponentFixture<PblTrnRaisedebitnoteSelectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnRaisedebitnoteSelectComponent]
    });
    fixture = TestBed.createComponent(PblTrnRaisedebitnoteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
