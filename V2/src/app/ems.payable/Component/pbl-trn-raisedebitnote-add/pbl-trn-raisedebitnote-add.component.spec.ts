import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnRaisedebitnoteAddComponent } from './pbl-trn-raisedebitnote-add.component';

describe('PblTrnRaisedebitnoteAddComponent', () => {
  let component: PblTrnRaisedebitnoteAddComponent;
  let fixture: ComponentFixture<PblTrnRaisedebitnoteAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnRaisedebitnoteAddComponent]
    });
    fixture = TestBed.createComponent(PblTrnRaisedebitnoteAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
